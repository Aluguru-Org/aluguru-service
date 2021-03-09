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
            Assert.Throws<DomainException>(() => new Category("", "video-games", false, null));
        }

        [Fact]
        public void CreateCategory_WhenEmptyUri_ShouldThrowException()
        {
            Assert.Throws<DomainException>(() => new Category("Video Games", "", false, null));
        }

        [Fact]
        public void CreateCategory_WhenInvalidUri_ShouldThrowException()
        {
            Assert.Throws<DomainException>(() => new Category("Video Games", "Video Games", false, null));
        }

        [Fact]
        public void CreateCategory_WhenEmptyMainCategoryId_ShouldThrowException()
        {
            Assert.Throws<DomainException>(() => new Category("Video Games", "video-games", false, Guid.Empty));
        }
    }
}
