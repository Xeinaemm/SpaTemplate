// -----------------------------------------------------------------------
// <copyright file="RepositoryShould.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Tests.IntegrationTests
{
    using SpaTemplate.Core.FacultyContext;
    using SpaTemplate.Tests.Helpers;
    using Xunit;

    public class RepositoryShould
    {
        [Fact]
        public void AddItemAndSetId()
        {
            var repository = DbContextHelper.GetRepository();
            var sut = new Student();

            Assert.True(repository.AddEntity(sut));
            var newItem = repository.GetFirstOrDefault(new StudentSpecification(sut.Id));

            Assert.Equal(sut.Id, newItem.Id);
        }

        [Fact]
        public void DeleteItemAfterAddingIt()
        {
            var repository = DbContextHelper.GetRepository();
            var sut = new Student();
            repository.AddEntity(sut);
            Assert.True(repository.DeleteEntity(sut));
        }

        [Fact]
        public void ExistsItemAfterAddingIt()
        {
            var repository = DbContextHelper.GetRepository();
            var sut = new Student();
            Assert.True(repository.AddEntity(sut));
            Assert.True(repository.ExistsEntity<Student>(sut.Id));
        }
    }
}