// -----------------------------------------------------------------------
// <copyright file="ConsentController.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.IdP
{
    using System.Threading.Tasks;
    using IdentityServer4.Stores;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Xeinaemm.AspNetCore;
    using Xeinaemm.AspNetCore.Identity.IdentityServer;

    [SecurityHeaders]
    [Authorize]
    public class ConsentController : Controller
    {
        private readonly IClientStore clientStore;
        private readonly IIdentityServerService identityServerService;

        public ConsentController(
            IClientStore clientStore,
            IIdentityServerService identityServerService)
        {
            this.clientStore = clientStore;
            this.identityServerService = identityServerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var vm = await this.identityServerService.BuildConsentViewModelAsync(returnUrl).ConfigureAwait(false);
            return vm != null ? this.View(nameof(this.Index), vm) : this.View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConsentInputModel model)
        {
            var result = await this.identityServerService.ProcessConsentAsync(model, this.User).ConfigureAwait(false);

            if (result.IsRedirect)
            {
                return await this.clientStore.IsPkceClientAsync(result.ClientId).ConfigureAwait(false)
                    ? this.View("Redirect", new RedirectViewModel { RedirectUrl = result.RedirectUri })
                    : (IActionResult)this.Redirect(result.RedirectUri);
            }

            if (result.HasValidationError)
                this.ModelState.AddModelError(string.Empty, result.ValidationError);

            return result.ShowView ? this.View(nameof(this.Index), result.ViewModel) : this.View("Error");
        }
    }
}