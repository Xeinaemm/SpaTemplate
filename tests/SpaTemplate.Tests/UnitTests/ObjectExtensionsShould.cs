// -----------------------------------------------------------------------
// <copyright file="ObjectExtensionsShould.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Tests.UnitTests
{
    using System.Collections.Generic;
    using Xeinaemm.Domain;
    using Xeinaemm.Hateoas;
    using Xunit;

    public class ObjectExtensionsShould
    {
        [Theory]
        [InlineData("Fizz", "Fizz")]
        [InlineData("fizz", "Fizz")]
        [InlineData("Fizz  ", "Fizz")]
        [InlineData("fizz  ", "Fizz")]
        public void ShapeData(string field, string key)
        {
            var sut = new DummyEntity();
            var list = sut.ShapeDataObject(field) as IDictionary<string, object>;

            Assert.Equal(sut.Fizz, list?[key]);
            Assert.Throws<KeyNotFoundException>(() => list?["Buzz"]);
            Assert.Throws<KeyNotFoundException>(() => list?["Id"]);
        }

        [Fact]
        public void ReturnsEverythingFieldIsNullOrWhiteSpace()
        {
            var sut = new DummyEntity();
            var list = sut.ShapeDataObject() as IDictionary<string, object>;

            Assert.Equal(sut.Fizz, list?["Fizz"]);
            Assert.Equal(sut.Buzz, list?["Buzz"]);
            Assert.Equal(sut.Id, list?["Id"]);
        }

        private class DummyEntity : BaseEntity
        {
            public string Buzz { get; }

            public string Fizz { get; }
        }
    }
}