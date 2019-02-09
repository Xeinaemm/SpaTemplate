// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Web.Quartz
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    public static class Program
    {
#pragma warning disable IDISP004 // Don't ignore return value of type IDisposable.
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();
#pragma warning restore IDISP004 // Don't ignore return value of type IDisposable.

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
