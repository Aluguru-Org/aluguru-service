using Mubbi.Marketplace.Domain.Core.Exceptions;
using Mubbi.Marketplace.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mubbi.Marketplace.UnitTests.ValueObjects
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
