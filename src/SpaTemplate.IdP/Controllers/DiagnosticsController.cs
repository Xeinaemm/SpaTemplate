// -----------------------------------------------------------------------
// <copyright file="DiagnosticsController.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.IdP
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Xeinaemm.AspNetCore;
    using Xeinaemm.AspNetCore.Identity.IdentityServer;

    [SecurityHeaders]
    [Authorize]
    public class DiagnosticsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var localAddresses = new string[] { "127.0.0.1", "::1", this.HttpContext.Connection.LocalIpAddress.ToString() };
            return !localAddresses.Contains(this.HttpContext.Connection.RemoteIpAddress.ToString())
                ? (IActionResult)this.NotFound()
                : this.View(new DiagnosticsViewModel(await this.HttpContext.AuthenticateAsync().ConfigureAwait(false)));
        }
    }
}