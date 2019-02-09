// -----------------------------------------------------------------------
// <copyright file="IPeopleCollectionApiShould.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Functional.Tests.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Autofac;
    using FluentAssertions;
    using Refit;
    using SpaTemplate.Application.Setup.ContainerTasks;
    using SpaTemplate.Contracts.Api;
    using SpaTemplate.Contracts.Models;
    using SpaTemplate.Infrastructure.Api;
    using SpaTemplate.Tests.Helpers;
    using Xeinaemm.Tests.Common.Attributes;
    using Xunit;

    public class IPeopleCollectionApiShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly IPeopleCollectionApi api;

        public IPeopleCollectionApiShould(CustomWebApplicationFactory<Startup> factory)
        {
            using var scope = InitializeContainer.InitializeTests(factory.CreateClientWithDefaultRequestHeaders()).BeginLifetimeScope();
            this.api = scope.Resolve<IPeopleCollectionApi>();
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateCollectionAfterValidPostAsync(IEnumerable<StudentForCreationDto> dtos)
        {
            var post = await this.api.CreateStudentCollection(dtos);
            post.Count().Should().Be(3);
        }

        [Fact]
        public async Task ReturnsBadRequestNoIdsAsync()
        {
            try
            {
                var post = await this.api.GetStudentCollection(new List<Guid> { Guid.Empty, Guid.Empty }.ToArray());
            }
            catch (ApiException validationException)
            {
                validationException.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Theory]
        [AutoMoqData]
        public async Task ReturnsCollectionAfterValidGetAsync(IEnumerable<StudentForCreationDto> dtos)
        {
            var post = await this.api.CreateStudentCollection(dtos);
            var get = await this.api.GetStudentCollection(post.Select(x => x.Id).Take(3));
            post.Should().AllBeEquivalentTo(get);
        }

        [Fact]
        public async Task ReturnsNotFoundIdsNotExistAsync()
        {
            var ids = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
            };

            try
            {
                var get = await this.api.GetStudentCollection(ids);
            }
            catch (ApiException validationException)
            {
                validationException.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }
    }
}
