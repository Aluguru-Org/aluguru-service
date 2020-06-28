using System;
using Xunit;

namespace Mubbi.Marketplace.Catalog.Domain.Tests
{
    public class CategoryTests
    {
        [Fact]
        public void CreateCategory_WhenEmptyName_ShouldThrowException()
        {
            Assert.Throws<Exception>(() => new Category("", 1000));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateCategory_WhenCodeSmallerOrEqualThanZero_ShouldThrowException(int category)
        {
            Assert.Throws<Exception>(() => new Category("Toys", category));
        }
    }
}
