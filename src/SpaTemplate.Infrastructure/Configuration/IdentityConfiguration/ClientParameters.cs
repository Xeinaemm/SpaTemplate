// -----------------------------------------------------------------------
// <copyright file="ClientParameters.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Infrastructure
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xeinaemm.AspNetCore.Identity.IdentityServer;

    public class ClientParameters : IClientParameters
    {
        public ClientParameters(string clientSecret, string clientAuthority)
        {
            this.Secret = clientSecret;
            this.Authority = clientAuthority;
        }

        public string Name { get; } = "E29B3139-AB17-485A-9712-F9F70777CB0A";

        public string DisplayName { get; } = "SpaTemplate.Web.Core";

        public string Secret { get; }

        public string Authority { get; }

        public IEnumerable<IIdentityResource> Resources { get; } = new Collection<IIdentityResource>
        {
            new Roles(),
        };

        public IEnumerable<string> ApisNames { get; } = new Collection<string>
        {
            new ApiParameters(string.Empty, string.Empty).Name,
        };
    }
}
