// -----------------------------------------------------------------------
// <copyright file="StudentServiceShould.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Tests.IntegrationTests
{
    using System;
    using System.Linq;
    using SpaTemplate.Contracts.Parameters;
    using SpaTemplate.Core.FacultyContext;
    using SpaTemplate.Core.SharedKernel;
    using SpaTemplate.Infrastructure.FacultyContext;
    using SpaTemplate.Tests.Helpers;
    using Xeinaemm.Hateoas;
    using Xeinaemm.Tests.Common.Attributes;
    using Xunit;

    public class StudentServiceShould
    {
        [Theory]
        [AutoMoqData]
        public void AssignableFromIPagedList(StudentService sut) => Assert.IsAssignableFrom<IStudentService>(sut);

        [Theory]
        [AutoMoqData]
        public void ReturnsChangedPaginationWithSpecifiedPageNumber(PropertyMappingService property)
        {
            var parameters = new StudentParameters { PageSize = 2, PageNumber = 2 };
            var sut = new StudentService(DbContextHelper.SeedRepo(property), new TypeHelperService(), new PropertyMappingService());

            var actual = sut.GetPagedList(parameters);

            Assert.Equal(2, actual.CurrentPage);
        }

        [Theory]
        [AutoMoqData]
        public void ReturnsCorrectOrderByDescription(PropertyMappingService property)
        {
            var parameters = new StudentParameters
            {
                OrderBy = "surname",
            };
            var sut = new StudentService(DbContextHelper.SeedRepo(property), new TypeHelperService(), new PropertyMappingService());

            var actual = sut.GetPagedList(parameters);

            Assert.Equal("Buzz", actual?[0]?.Surname);
        }

        [Theory]
        [AutoMoqData]
        public void ReturnsCorrectOrderByTitle(PropertyMappingService property)
        {
            var parameters = new StudentParameters
            {
                OrderBy = "Name",
            };
            var sut = new StudentService(DbContextHelper.SeedRepo(property), new TypeHelperService(), new PropertyMappingService());

            var actual = sut.GetPagedList(parameters);

            Assert.Equal("Dummy", actual?[0]?.Name);
        }

        [Theory]
        [InlineData("dummy", 2)]
        [InlineData("Test", 3)]
        public void ReturnsCorrectQueryList(string searchQuery, int totalCount)
        {
            var parameters = new StudentParameters
            {
                SearchQuery = searchQuery,
            };
            var sut = new StudentService(DbContextHelper.SeedRepo(new PropertyMappingService()), new TypeHelperService(), new PropertyMappingService());

            var actual = sut.GetPagedList(parameters);

            Assert.Equal(totalCount, actual.TotalCount);
        }

        [Theory]
        [InlineAutoMoqData(10, 1, 1)]
        [InlineAutoMoqData(2, 1, 3)]
        public void ReturnsCorrectTotalPages(int pageSize, int pageNumber, int totalPages, PropertyMappingService property)
        {
            var parameters = new StudentParameters
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
            };
            var sut = new StudentService(DbContextHelper.SeedRepo(property), new TypeHelperService(), new PropertyMappingService());

            var actual = sut.GetPagedList(parameters);

            Assert.Equal(totalPages, actual.TotalPages);
        }

        [Fact]
        public void ReturnsListEqualIds()
        {
            var sut = new StudentService(DbContextHelper.SeedRepo(new PropertyMappingService()), new TypeHelperService(), new PropertyMappingService());

            var ids = DbContextHelper.DummyGuidsArray();
            var actual = sut.GetCollection(ids);

            Assert.Equal(ids, actual.Select(x => x.Id));
        }

        [Theory]
        [AutoMoqData]
        public void ThrowsExceptionOrderById(PropertyMappingService property)
        {
            var parameters = new StudentParameters
            {
                OrderBy = "Id",
            };
            var sut = new StudentService(DbContextHelper.SeedRepo(property), new TypeHelperService(), new PropertyMappingService());

            Assert.Throws<ArgumentException>(() => sut.GetPagedList(parameters));
        }
    }
}