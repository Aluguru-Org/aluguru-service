using Aluguru.Marketplace.Catalog.Domain;
using System;
using Xunit;

namespace Aluguru.Marketplace.Catalog.UnitTests
{
    public class DimensionsTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateDimensions_WhenHeightSmallerOrEqualThanZero_ShouldThrowException(int height)
        {
            Assert.Throws<Exception>(() => new Dimensions(height, 1, 1));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateDimensions_WhenWidthSmallerOrEqualThanZero_ShouldThrowException(int width)
        {
            Assert.Throws<Exception>(() => new Dimensions(1, width, 1));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateDimensions_WhenDepthSmallerOrEqualThanZero_ShouldThrowException(int depth)
        {
            Assert.Throws<Exception>(() => new Dimensions(1, 1, depth));
        }
    }
}
