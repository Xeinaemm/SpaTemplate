// -----------------------------------------------------------------------
// <copyright file="IApiShould.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Functional.Tests.Api
{
    using System.Linq;
    using System.Threading.Tasks;
    using Autofac;
    using SpaTemplate.Application.Setup.ContainerTasks;
    using SpaTemplate.Contracts.Api;
    using SpaTemplate.Core.SharedKernel;
    using SpaTemplate.Infrastructure.Api;
    using SpaTemplate.Tests.Helpers;
    using Xeinaemm.Hateoas;
    using Xunit;

    public class IApiShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly IApi api;

        public IApiShould(CustomWebApplicationFactory<Startup> factory)
        {
            using var scope = InitializeContainer.InitializeTests(factory.CreateClientWithDefaultRequestHeaders()).BeginLifetimeScope();
            this.api = scope.Resolve<IApi>();
        }

        [Theory]
        [InlineData(Rel.Self, "GET", 0)]
        [InlineData(SpaTemplateRel.People, "GET", 1)]
        [InlineData(SpaTemplateRel.CreateStudent, "POST", 2)]
        public async Task ReturnsHateoasLinksRootAsync(string rel, string method, int number)
        {
            var result = (await this.api.GetRoot()).ToArray();
            Assert.Equal(3, result.Count());

            Assert.Equal(rel, result[number].Rel);
            Assert.Equal(method, result[number].Method);
        }
    }
}
