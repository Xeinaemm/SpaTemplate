// -----------------------------------------------------------------------
// <copyright file="ConfigurationExtensions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.IdP
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {
        public static string GetApiSecurityString(this IConfiguration config) =>
            config.GetSection("Security")["SecurityKeyApi"];

        public static string GetApiAuthorityString(this IConfiguration config) =>
            config.GetSection("Security")["AuthorityApi"];

        public static string GetClientSecurityString(this IConfiguration config) =>
            config.GetSection("Security")["SecurityKeyClient"];

        public static string GetClientAuthorityString(this IConfiguration config) =>
            config.GetSection("Security")["AuthorityClient"];
    }
}
