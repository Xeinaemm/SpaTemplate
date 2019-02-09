// -----------------------------------------------------------------------
// <copyright file="Roles.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Infrastructure
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xeinaemm.AspNetCore.Identity.IdentityServer;

    public class Roles : IIdentityResource
    {
        public string Name { get; } = nameof(Roles);

        public string DisplayName { get; } = "Your role(s)";

        public IEnumerable<string> ClaimTypes { get; } = new Collection<string> { "role" };
    }
}
