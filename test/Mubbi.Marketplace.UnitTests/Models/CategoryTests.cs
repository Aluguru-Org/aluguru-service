using Mubbi.Marketplace.Domain.Core.Exceptions;
using Mubbi.Marketplace.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mubbi.Marketplace.UnitTests.Models
{
    public class CategoryTests
    {
        [Fact]
        public void CreateCategory_WhenEmptyName_ShouldThrowException()
        {
            Assert.Throws<DomainException>(() => new Category("", 1000));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateCategory_WhenCodeSmallerOrEqualThanZero_ShouldThrowException(int category)
        {
            Assert.Throws<DomainException>(() => new Category("Toys", category));
        }
    }
}
