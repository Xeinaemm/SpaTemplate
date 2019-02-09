// -----------------------------------------------------------------------
// <copyright file="PagedListShould.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Tests.UnitTests
{
    using System.Collections.Generic;
    using Xeinaemm.Common;
    using Xunit;

    public class PagedListShould
    {
        [Theory]
        [InlineData(1, 1, 5, true, false, 5)]
        [InlineData(1, 2, 5, true, false, 3)]
        [InlineData(2, 1, 5, true, true, 5)]
        [InlineData(2, 2, 5, true, true, 3)]
        [InlineData(3, 2, 5, false, true, 3)]
        [InlineData(5, 1, 5, false, true, 5)]
        public void ReturnsPagedListCorrectPagination(int pageNumber, int pageSize, int totalCount, bool hasNext, bool hasPrevious, int totalPages)
        {
            var pagedList = new PagedListCollection<DummyEntity>(DummyList(), pageNumber, pageSize);

            Assert.Equal(pageSize, pagedList.PageSize);
            Assert.Equal(pageNumber, pagedList.CurrentPage);
            Assert.Equal(totalCount, pagedList.TotalCount);
            Assert.Equal(hasNext, pagedList.HasNext);
            Assert.Equal(hasPrevious, pagedList.HasPrevious);
            Assert.Equal(totalPages, pagedList.TotalPages);
        }

        private static List<DummyEntity> DummyList() => new List<DummyEntity>
        {
            new DummyEntity(),
            new DummyEntity(),
            new DummyEntity(),
            new DummyEntity(),
            new DummyEntity(),
        };
    }
}