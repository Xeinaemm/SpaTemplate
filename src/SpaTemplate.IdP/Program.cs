// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.IdP
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Xeinaemm.AspNetCore.Identity.IdentityServer;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            try
            {
                using var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var provider = scope.ServiceProvider;
                await provider.EnsureIdentitySeedDataAsync<IdentityUser, CustomIdentityDbContext>(new IdentitySeedData(provider.GetRequiredService<IConfiguration>()), true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
