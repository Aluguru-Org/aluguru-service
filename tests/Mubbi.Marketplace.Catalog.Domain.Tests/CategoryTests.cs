using Mubbi.Marketplace.Domain;
using System;
using Xunit;

namespace Mubbi.Marketplace.Catalog.Domain.Tests
{
    public class CategoryTests
    {
        [Fact]
        public void CreateCategory_WhenEmptyName_ShouldThrowException()
        {
            Assert.Throws<DomainException>(() => new Category("", 1000, null));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateCategory_WhenCodeSmallerOrEqualThanZero_ShouldThrowException(int category)
        {
            Assert.Throws<DomainException>(() => new Category("Toys", category, null));
        }

        [Fact]
        public void CreateCategory_WhenEmptyMainCategoryId_ShouldThrowException()
        {
            Assert.Throws<DomainException>(() => new Category("Toys", 1000, Guid.Empty));
        }
    }
}
