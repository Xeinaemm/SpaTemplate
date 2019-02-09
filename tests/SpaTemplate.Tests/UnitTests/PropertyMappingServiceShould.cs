// -----------------------------------------------------------------------
// <copyright file="PropertyMappingServiceShould.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using SpaTemplate.Core.SharedKernel;
    using Xeinaemm.Domain;
    using Xeinaemm.Hateoas;
    using Xeinaemm.Tests.Common.Attributes;
    using Xunit;

    public class PropertyMappingServiceShould
    {
        [Theory]
        [InlineAutoMoqData("Dummy")]
        public void ReturnsTrueValidFields(string field, PropertyMappingService property) => Assert.True(property.ValidMappingExistsFor<DummyEntityDto, DummyEntity>(field));

        [Fact]
        public void BeAssignableFromInterface() => Assert.IsAssignableFrom<IPropertyMappingService>(new PropertyMappingService());

        private class DummyEntity : BaseEntity
        {
        }

        private class DummyEntityDto : IDto
        {
            public List<BaseDomainEvent> Events { get; }

            public Guid Id { get; set; }
        }
    }
}