// -----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.IdP
{
    using System;
    using System.Threading.Tasks;
    using IdentityServer4.Events;
    using IdentityServer4.Extensions;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using IdentityServer4.Stores;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Xeinaemm.AspNetCore;
    using Xeinaemm.AspNetCore.Identity.IdentityServer;

    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IClientStore clientStore;
        private readonly IIdentityServerService identityServerService;
        private readonly IEventService events;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IIdentityServerService identityServerService,
            IEventService events)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.interaction = interaction;
            this.clientStore = clientStore;
            this.identityServerService = identityServerService;
            this.events = events;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var vm = await this.identityServerService.BuildLoginViewModelAsync(returnUrl).ConfigureAwait(false);

            return vm.IsExternalLoginOnly
                ? this.RedirectToAction("Challenge", "External", new { provider = vm.ExternalLoginScheme, returnUrl })
                : (IActionResult)this.View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            var context = await this.interaction.GetAuthorizationContextAsync(model.ReturnUrl).ConfigureAwait(false);

            if (button != "login")
            {
                if (context != null)
                {
                    await this.interaction.GrantConsentAsync(context, ConsentResponse.Denied).ConfigureAwait(false);

                    return await this.clientStore.IsPkceClientAsync(context.ClientId).ConfigureAwait(false)
                        ? this.View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl })
                        : (IActionResult)this.Redirect(model.ReturnUrl);
                }

                return this.Redirect("~/");
            }

            if (this.ModelState.IsValid)
            {
                var result = await this.signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, true).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    var user = await this.userManager.FindByNameAsync(model.Username).ConfigureAwait(false);
                    await this.events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName)).ConfigureAwait(false);

                    if (context != null)
                    {
                        return await this.clientStore.IsPkceClientAsync(context.ClientId).ConfigureAwait(false)
                            ? this.View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl })
                            : (IActionResult)this.Redirect(model.ReturnUrl);
                    }

                    if (this.Url.IsLocalUrl(model.ReturnUrl)) return this.Redirect(model.ReturnUrl);
                    if (string.IsNullOrEmpty(model.ReturnUrl)) return this.Redirect("~/");

                    throw new Exception("invalid return URL");
                }

                await this.events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials")).ConfigureAwait(false);
                this.ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            var vm = await this.identityServerService.BuildLoginViewModelAsync(model).ConfigureAwait(false);
            return this.View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = await this.identityServerService.BuildLogoutViewModelAsync(logoutId, this.User).ConfigureAwait(false);
            return !vm.ShowLogoutPrompt ? await this.Logout(vm).ConfigureAwait(false) : this.View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            var vm = await this.identityServerService.BuildLoggedOutViewModelAsync(model.LogoutId, this.User, this.HttpContext).ConfigureAwait(false);

            if (this.User?.Identity.IsAuthenticated == true)
            {
                await this.signInManager.SignOutAsync().ConfigureAwait(false);

                await this.events.RaiseAsync(new UserLogoutSuccessEvent(this.User.GetSubjectId(), this.User.GetDisplayName())).ConfigureAwait(false);
            }

            if (vm.TriggerExternalSignout)
            {
                var url = this.Url.Action(nameof(this.Logout), new { logoutId = vm.LogoutId });

                return this.SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return this.View("LoggedOut", vm);
        }
    }
}