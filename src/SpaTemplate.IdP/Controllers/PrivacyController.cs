// -----------------------------------------------------------------------
// <copyright file="PrivacyController.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.IdP.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class PrivacyController : Controller
    {
        public IActionResult Index() => this.View();
    }
}
