using Mubbi.Marketplace.Catalog.Domain.ValueObjects;
using Mubbi.Marketplace.Shared.DomainObjects;
using Xunit;

namespace Mubbi.Marketplace.Catalog.Domain.Tests
{
    public class DimensionsTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateDimensions_WhenHeightSmallerOrEqualThanZero_ShouldThrowException(int height)
        {
            Assert.Throws<DomainException>(() => new Dimensions(height, 1, 1));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateDimensions_WhenWidthSmallerOrEqualThanZero_ShouldThrowException(int width)
        {
            Assert.Throws<DomainException>(() => new Dimensions(1, width, 1));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateDimensions_WhenDepthSmallerOrEqualThanZero_ShouldThrowException(int depth)
        {
            Assert.Throws<DomainException>(() => new Dimensions(1, 1, depth));
        }
    }
}
