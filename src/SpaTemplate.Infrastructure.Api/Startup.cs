// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Infrastructure.Api
{
    using System;
    using System.Collections.Generic;
    using AspNetCoreRateLimit;
    using Autofac;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using SpaTemplate.Application.Setup.ContainerTasks;
    using Xeinaemm.AspNetCore;
    using Xeinaemm.AspNetCore.Api;
    using Xeinaemm.AspNetCore.Data;
    using Xeinaemm.AspNetCore.Identity.IdentityServer;
    using Xeinaemm.AspNetCore.Swagger;
    using Xeinaemm.Quartz;

    public class Startup
    {
        private readonly IWebHostEnvironment env;

        public Startup(
            IConfiguration configuration, IWebHostEnvironment env)
        {
            this.Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            if (this.env.EnvironmentName == "Production")
                services.AddCustomDbContext<ApplicationDbContext>(this.Configuration.GetConnectionString());
            else
                services.AddCustomInMemoryDbContext<ApplicationDbContext>("api");

            services.AddCustomApiControllers();
            //services.AddControllers(opt => opt.Filters.Add(new AllowAnonymousFilter()));
            services.AddCustomApiBehavior();
            services.AddCustomVersionedApiExplorer();
            services.AddCustomApiAuthentication(new ApiParameters(this.Configuration.GetSecurityString(), this.Configuration.GetIdPAuthorityString()));
            services.AddCustomApiVersioning();
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.CustomSwaggerDoc(services, nameof(Api), OpenApiInfoCommon.Create(nameof(Api), " "));
                setupAction.CustomSecurityDefinition();
                setupAction.CustomSecurityRequirement();
                setupAction.CustomDocInclusionPredicate(nameof(Api));
                setupAction.CustomXmlComments<EmptyClassSpaTemplateInfrastructureApi>();
            });

            services.AddOpenApiDocument();
            services.AddCustomHttpCacheHeaders();
            services.AddMemoryCache();
            services.AddCustomIpRateLimitOptions(new List<RateLimitRule>());
            services.AddCustomAutoMapper();

            return services.InitializeApi(builder => builder.Register(_ => new QuartzService(this.Configuration.GetConnectionString())).As<IQuartzService>().SingleInstance());
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider apiVersionDescriptionProvider, IWebHostEnvironment env)
        {
            app.UseIpRateLimiting();
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            SwaggerBuilderExtensions.UseSwagger(app);
            app.UseCustomSwaggerUI(apiVersionDescriptionProvider, nameof(Api));
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseHttpCacheHeaders();
            app.UseCookiePolicy();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}