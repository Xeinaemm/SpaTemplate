// -----------------------------------------------------------------------
// <copyright file="ExternalController.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.IdP
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using IdentityModel;
    using IdentityServer4.Events;
    using IdentityServer4.Services;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Xeinaemm.AspNetCore;
    using Xeinaemm.AspNetCore.Identity.IdentityServer;

    [SecurityHeaders]
    [AllowAnonymous]
    public class ExternalController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IIdentityServerService identityServerService;
        private readonly IEventService events;

        public ExternalController(
            SignInManager<IdentityUser> signInManager,
            IIdentityServerInteractionService interaction,
            IIdentityServerService identityServerService,
            IEventService events)
        {
            this.signInManager = signInManager;
            this.interaction = interaction;
            this.identityServerService = identityServerService;
            this.events = events;
        }

        [HttpGet]
        public async Task<IActionResult> Challenge(string provider, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

            if (!this.Url.IsLocalUrl(returnUrl) && !this.interaction.IsValidReturnUrl(returnUrl)) throw new Exception("invalid return URL");

            if (provider == AccountOptions.WindowsAuthenticationSchemeName)
                return await this.identityServerService.ProcessWindowsLoginAsync(returnUrl, this.HttpContext, this.Url, this, nameof(this.Callback)).ConfigureAwait(false);

            var props = new AuthenticationProperties
            {
                RedirectUri = this.Url.Action(nameof(this.Callback)),
                Items =
                    {
                        { nameof(returnUrl), returnUrl },
                        { "scheme", provider },
                    },
            };

            return this.Challenge(props, provider);
        }

        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            var result = await this.HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);
            if (result?.Succeeded != true) throw new Exception("External authentication error");

            var (user, provider, providerUserId, claims) = await this.identityServerService.FindUserFromExternalProviderAsync(result).ConfigureAwait(false);
            if (user == null) user = await this.identityServerService.AutoProvisionUserAsync(provider, providerUserId, claims).ConfigureAwait(false);

            var additionalLocalClaims = new List<Claim>();
            var localSignInProps = new AuthenticationProperties();
            this.identityServerService.ProcessLoginCallbackForOidc(result, additionalLocalClaims, localSignInProps);

            var principal = await this.signInManager.CreateUserPrincipalAsync(user).ConfigureAwait(false);
            additionalLocalClaims.AddRange(principal.Claims);
            var name = principal.FindFirst(JwtClaimTypes.Name)?.Value ?? user.Id;
            await this.events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.Id, name)).ConfigureAwait(false);
            await this.HttpContext.SignInAsync(user.Id, name, provider, localSignInProps, additionalLocalClaims.ToArray()).ConfigureAwait(false);

            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

            var returnUrl = result.Properties.Items["returnUrl"];
            return this.interaction.IsValidReturnUrl(returnUrl) || this.Url.IsLocalUrl(returnUrl)
                ? this.Redirect(returnUrl)
                : this.Redirect("~/");
        }
    }
}