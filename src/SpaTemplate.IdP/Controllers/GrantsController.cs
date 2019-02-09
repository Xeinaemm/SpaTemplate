// -----------------------------------------------------------------------
// <copyright file="GrantsController.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.IdP
{
    using System.Threading.Tasks;
    using IdentityServer4.Events;
    using IdentityServer4.Extensions;
    using IdentityServer4.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Xeinaemm.AspNetCore;
    using Xeinaemm.AspNetCore.Identity.IdentityServer;

    [SecurityHeaders]
    [Authorize]
    public class GrantsController : Controller
    {
        private readonly IIdentityServerInteractionService interaction;
        private readonly IIdentityServerService identityServerService;
        private readonly IEventService events;

        public GrantsController(
            IIdentityServerInteractionService interaction,
            IIdentityServerService identityServerService,
            IEventService events)
        {
            this.interaction = interaction;
            this.identityServerService = identityServerService;
            this.events = events;
        }

        [HttpGet]
        public async Task<IActionResult> Index() =>
            this.View(nameof(this.Index), await this.identityServerService.BuildGrantsViewModelAsync().ConfigureAwait(false));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Revoke(string clientId)
        {
            await this.interaction.RevokeUserConsentAsync(clientId).ConfigureAwait(false);
            await this.events.RaiseAsync(new GrantsRevokedEvent(this.User.GetSubjectId(), clientId)).ConfigureAwait(false);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}