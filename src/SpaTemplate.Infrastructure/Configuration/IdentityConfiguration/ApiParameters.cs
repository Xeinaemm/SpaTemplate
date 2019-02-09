// -----------------------------------------------------------------------
// <copyright file="ApiParameters.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Infrastructure
{
    using System.Collections.Generic;
    using Xeinaemm.AspNetCore.Identity.IdentityServer;

    public class ApiParameters : IApiParameters
    {
        public ApiParameters()
        {
        }

        public ApiParameters(string secret, string authority)
        {
            this.Secret = secret;
            this.Authority = authority;
        }

        public string Name { get; } = "EA3DAD10-821E-4937-A503-E72D3549E0E8";

        public string DisplayName { get; } = "SpaTemplate.Infrastructure.Api";

        public string Secret { get; } = string.Empty;

        public string Authority { get; } = string.Empty;

        public IEnumerable<string> ClaimTypes { get; } = new Roles().ClaimTypes;
    }
}
