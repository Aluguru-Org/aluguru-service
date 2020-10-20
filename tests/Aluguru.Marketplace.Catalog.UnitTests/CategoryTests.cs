using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using System;
using Xunit;

namespace Aluguru.Marketplace.Catalog.UnitTests
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
