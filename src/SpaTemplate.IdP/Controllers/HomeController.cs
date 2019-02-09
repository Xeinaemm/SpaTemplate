// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.IdP
{
    using System.Threading.Tasks;
    using IdentityServer4.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Xeinaemm.AspNetCore;
    using Xeinaemm.AspNetCore.Identity.IdentityServer;

    [SecurityHeaders]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService interaction;
        private readonly IWebHostEnvironment environment;

        public HomeController(IIdentityServerInteractionService interaction, IWebHostEnvironment environment)
        {
            this.interaction = interaction;
            this.environment = environment;
        }

        public IActionResult Index() => this.environment.EnvironmentName == "Development" ? this.View() : (IActionResult)this.NotFound();

        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            var message = await this.interaction.GetErrorContextAsync(errorId).ConfigureAwait(false);
            if (message != null)
            {
                vm.Error = message;

                if (this.environment.EnvironmentName == "Development")
                {
                    message.ErrorDescription = null;
                }
            }

            return this.View(nameof(this.Error), vm);
        }
    }
}