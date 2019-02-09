// -----------------------------------------------------------------------
// <copyright file="CustomWebApplicationFactoryExtensions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Tests.Helpers
{
    using System.Net.Http;
    using SpaTemplate.Infrastructure.Api;
    using Xeinaemm.Hateoas;

    public static class CustomWebApplicationFactoryExtensions
    {
        public static HttpClient CreateClientWithDefaultRequestHeaders(
            this CustomWebApplicationFactory<Startup> factory)
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation(Header.XRealIp, "127.0.0.1");

            return client;
        }
    }
}