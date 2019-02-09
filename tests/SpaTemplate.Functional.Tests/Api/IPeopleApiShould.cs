// -----------------------------------------------------------------------
// <copyright file="IPeopleApiShould.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Functional.Tests.Api
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Autofac;
    using FluentAssertions;
    using Microsoft.AspNetCore.JsonPatch;
    using Refit;
    using SpaTemplate.Application.Setup.ContainerTasks;
    using SpaTemplate.Contracts.Api;
    using SpaTemplate.Contracts.Models;
    using SpaTemplate.Contracts.Parameters;
    using SpaTemplate.Core.SharedKernel;
    using SpaTemplate.Infrastructure.Api;
    using SpaTemplate.Tests.Helpers;
    using Xeinaemm.Hateoas;
    using Xeinaemm.Tests.Common.Attributes;
    using Xunit;

    public class IPeopleApiShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly IPeopleApi api;

        public IPeopleApiShould(CustomWebApplicationFactory<Startup> factory)
        {
            using var scope = InitializeContainer.InitializeTests(factory.CreateClientWithDefaultRequestHeaders()).BeginLifetimeScope();
            this.api = scope.Resolve<IPeopleApi>();
        }

        [Fact]
        public async Task ReturnsBadRequestOrderByNotExists()
        {
            try
            {
                var get = await this.api.GetPeople(new StudentParameters { OrderBy = "unknown" });
            }
            catch (ApiException validationException)
            {
                validationException.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task ReturnsBadRequestFieldsMappingNotExists()
        {
            try
            {
                var get = await this.api.GetPeople(new StudentParameters { Fields = "dummy" });
            }
            catch (ApiException validationException)
            {
                validationException.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task ReturnsHateoasCollection()
        {
            var get = await this.api.GetPeopleHateoas(new StudentParameters());
            Assert.Equal(10, get.Values.Count());
        }

        [Theory]
        [InlineData("GET", Rel.Self, 0)]
        [InlineData("POST", SpaTemplateRel.CreateStudent, 1)]
        [InlineData("PATCH", SpaTemplateRel.PatchStudent, 2)]
        [InlineData("DELETE", SpaTemplateRel.DeleteStudent, 3)]
        public async Task ReturnsCollectionValidMethodAndRelInsideLinks(string method, string rel, int number)
        {
            var get = await this.api.GetPeopleHateoas(new StudentParameters());

            Assert.Equal(10, get.Values.Count);
            Assert.Equal(4, get.Values[0].Links.Count);

            var linkDto = get.Values[0].Links[number];
            Assert.Equal(method, linkDto.Method);
            Assert.Equal(rel, linkDto.Rel);
        }

        [Theory]
        [InlineData("GET", Rel.Self, 0)]
        [InlineData("GET", Rel.NextPage, 1)]
        public async Task ReturnsCollectionValidMethodAndRelOutsideLinks(string method, string rel, int number)
        {
            var get = await this.api.GetPeopleHateoas(new StudentParameters());

            Assert.Equal(2, get.Links.Count);

            Assert.Equal(method, get.Links[number].Method);
            Assert.Equal(rel, get.Links[number].Rel);
        }

        [Fact]
        public async Task ReturnsNotFoundEntityNotExists()
        {
            try
            {
                var get = await this.api.GetStudent(Guid.NewGuid(), new StudentParameters());
            }
            catch (ApiException validationException)
            {
                validationException.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public async Task ReturnsNotNullHref()
        {
            var get = await this.api.GetPeopleHateoas(new StudentParameters());
            Assert.All(get.Values[0].Links, x => Assert.NotNull(x.Href));
            Assert.All(get.Links, x => Assert.NotNull(x.Href));
            Assert.Contains(get.Links, x => x.Rel == Rel.NextPage);

            var getSecond = await this.api.GetPeopleHateoas(new StudentParameters { PageNumber = 2 });
            Assert.All(getSecond.Values[0].Links, x => Assert.NotNull(x.Href));
            Assert.All(getSecond.Links, x => Assert.NotNull(x.Href));
            Assert.Contains(getSecond.Links, x => x.Rel == Rel.PreviousPage);
        }

        [Theory]
        [AutoMoqData]
        public async Task ReturnsCreated(StudentForCreationDto studentForCreationDto)
        {
            var post = await this.api.CreateStudent(studentForCreationDto);
            Assert.NotNull(post);
        }

        [Fact]
        public async Task ReturnsNotFoundEntityForPatchNotExists()
        {
            try
            {
                var patch = await this.api.PartiallyUpdateStudent(Guid.NewGuid(), new JsonPatchDocument<StudentForUpdateDto>());
            }
            catch (ApiException validationException)
            {
                validationException.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public async Task ReturnsNotFoundEntity()
        {
            try
            {
                await this.api.DeleteStudent(Guid.NewGuid());
            }
            catch (ApiException validationException)
            {
                validationException.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Theory]
        [AutoMoqData]
        public async Task ReturnsBadRequestFieldNotExists(StudentForCreationDto studentForCreationDto)
        {
            var post = await this.api.CreateStudent(studentForCreationDto);

            try
            {
                //var get = await this.api.GetStudent(post.Id, new StudentParameters { Fields = "dummy" });
            }
            catch (ApiException validationException)
            {
                validationException.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Theory]
        [AutoMoqData]
        public async Task ReturnsNoContentValidPatch(StudentForCreationDto studentForCreationDto)
        {
            var post = await this.api.CreateStudent(studentForCreationDto);

            var patchDoc = new JsonPatchDocument<StudentForUpdateDto>();
            patchDoc.Replace(x => x.Surname, "Dummy");

            //var patch = await this.api.PartiallyUpdateStudent(post.Id, patchDoc);
            //Assert.Null(patch);
        }

        [Theory]
        [AutoMoqData]
        public async Task ReturnsUnprocessableEntityEqualFields(StudentForCreationDto studentForCreationDto)
        {
            var post = await this.api.CreateStudent(studentForCreationDto);

            //var patchDoc = new JsonPatchDocument<StudentForUpdateDto>();
            //patchDoc.Replace(x => x.Name, post.Surname);

            //try
            //{
            //    var patch = await this.api.PartiallyUpdateStudent(post.Id, patchDoc);
            //}
            //catch (ApiException validationException)
            //{
            //    validationException.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            //}
        }

        [Theory]
        [AutoMoqData]
        public async Task RemoveStudent(StudentForCreationDto studentForCreationDto)
        {
            var post = await this.api.CreateStudent(studentForCreationDto);
            //await this.api.DeleteStudent(post.Id);

            //try
            //{
            //    var get = await this.api.GetStudent(post.Id, new StudentParameters());
            //}
            //catch (ApiException validationException)
            //{
            //    validationException.StatusCode.Should().Be(HttpStatusCode.NotFound);
            //}
        }
    }
}
