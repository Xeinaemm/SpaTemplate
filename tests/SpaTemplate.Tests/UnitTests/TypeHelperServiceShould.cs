// -----------------------------------------------------------------------
// <copyright file="TypeHelperServiceShould.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture.Xunit2;
    using Xeinaemm.Domain;
    using Xeinaemm.Hateoas;
    using Xunit;

    public class TypeHelperServiceShould
    {
        [Theory]
        [InlineAutoData("FizzBuzz!")]
        public void ReturnsFalseFieldNotExist(string fields)
        {
            var sut = new TypeHelperService();
            Assert.False(sut.TypeHasProperties<DummyEntity>(fields));
        }

        [Theory]
        [InlineAutoData(null)]
        [InlineAutoData("id")]
        [InlineAutoData("Fizz")]
        [InlineAutoData("id,Fizz, buzz")]
        [InlineAutoData("buzz     ")]
        public void ReturnsTrueNullOrCorrectFields(string fields)
        {
            var sut = new TypeHelperService();
            Assert.True(sut.TypeHasProperties<DummyEntity>(fields));
        }

        [Fact]
        public void BeAssignableFromInterface() => Assert.IsAssignableFrom<ITypeHelperService>(new TypeHelperService());

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
        private class DummyEntity : IDto
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
        {
            public string Buzz { get; set; }

            public List<BaseDomainEvent> Events { get; }

            public string Fizz { get; set; }

            public Guid Id { get; set; }
        }
    }
}