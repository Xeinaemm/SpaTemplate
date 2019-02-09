// -----------------------------------------------------------------------
// <copyright file="IdentitySeedData.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.IdP
{
    using System.Collections.Generic;
    using IdentityServer4.Models;
    using Microsoft.Extensions.Configuration;
    using SpaTemplate.Infrastructure;
    using Xeinaemm.AspNetCore.Identity.IdentityServer;

    public class IdentitySeedData : IIdentitySeedData
    {
        private readonly IConfiguration configuration;

        public IdentitySeedData(IConfiguration configuration)
        {
            this.configuration = configuration;
            var client = new ClientParameters(configuration.GetClientSecurityString(), this.configuration.GetClientAuthorityString());
            this.IdentityResources = new List<IClientParameters>
            {
                client,
            }.Identity();

            this.ApiResources = new List<IApiParameters>
            {
                new ApiParameters(),
            }.Api();

            this.Clients = new List<Client>
            {
                client.Mvc(),
            };

            this.Users = new List<IUser>
            {
                new Alice(),
                new Bob(),
            };
        }

        public IEnumerable<IdentityResource> IdentityResources { get; }

        public IEnumerable<ApiResource> ApiResources { get; }

        public IEnumerable<Client> Clients { get; }

        public IEnumerable<IUser> Users { get; }
    }
}
