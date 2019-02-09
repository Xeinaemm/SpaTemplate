// -----------------------------------------------------------------------
// <copyright file="DeviceController.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.IdP
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Xeinaemm.AspNetCore;
    using Xeinaemm.AspNetCore.Identity.IdentityServer;

    [Authorize]
    [SecurityHeaders]
    public class DeviceController : Controller
    {
        private readonly IIdentityServerService identityServerService;

        public DeviceController(IIdentityServerService identityServerService) =>
            this.identityServerService = identityServerService;

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "user_code")] string userCode)
        {
            if (string.IsNullOrWhiteSpace(userCode)) return this.View(nameof(this.UserCodeCapture));

            var vm = await this.identityServerService.BuildDeviceAuthorizationViewModelAsync(userCode).ConfigureAwait(false);
            if (vm == null) return this.View("Error");

            vm.ConfirmUserCode = true;
            return this.View("UserCodeConfirmation", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserCodeCapture(string userCode)
        {
            var vm = await this.identityServerService.BuildDeviceAuthorizationViewModelAsync(userCode).ConfigureAwait(false);
            return vm == null ? this.View("Error") : this.View("UserCodeConfirmation", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Callback(DeviceAuthorizationInputModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var result = await this.identityServerService.ProcessConsentAsync(model, this.User).ConfigureAwait(false);
            return result.HasValidationError ? this.View("Error") : this.View("Success");
        }
    }
}