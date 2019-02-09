// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.IdP
{
    using System;
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using SpaTemplate.Application.Setup.ContainerTasks;
    using Xeinaemm.AspNetCore;
    using Xeinaemm.AspNetCore.Identity.IdentityServer;

    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCustomCookiePolicy();
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddCustomIISOptions();

            if (this.env.EnvironmentName == "Production")
            {
                services.AddCustomIdentityServer<IdentityUser, CustomIdentityDbContext>(this.configuration.GetConnectionString(), Assembly.GetExecutingAssembly().GetName().Name)
                    .AddDeveloperSigningCredential();
            }
            else
            {
                services.AddCustomInMemoryIdentityServer<IdentityUser, CustomIdentityDbContext>(Assembly.GetExecutingAssembly().GetName().Name)
                    .AddDeveloperSigningCredential();
            }

            return services.InitializeIdp();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
