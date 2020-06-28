using System;
using Xunit;

namespace Mubbi.Marketplace.Register.Domain.Tests
{
    public class NameTests
    {
        [Fact]
        public void CreateName_WhenEmptyFirstName_ShouldThrowDomainException()
        {
            Assert.Throws<Exception>(() => new Name("", "Almeida"));
        }

        [Fact]
        public void CreateName_WhenEmptyLastName_ShouldThrowDomainException()
        {
            Assert.Throws<Exception>(() => new Name("Felipe", ""));
        }
    }
}
