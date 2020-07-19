using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using System;
using Xunit;

namespace Mubbi.Marketplace.Catalog.UnitTests
{
    public class CategoryTests
    {
        [Fact]
        public void CreateCategory_WhenEmptyName_ShouldThrowException()
        {
            Assert.Throws<DomainException>(() => new Category("", null));
        }

        [Fact]
        public void CreateCategory_WhenEmptyMainCategoryId_ShouldThrowException()
        {
            Assert.Throws<DomainException>(() => new Category("Toys", Guid.Empty));
        }
    }
}
