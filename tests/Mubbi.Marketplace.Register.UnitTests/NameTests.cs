using Mubbi.Marketplace.Register.Domain;
using System;
using Xunit;

namespace Mubbi.Marketplace.Register.UnitTests
{ 
    public class NameTests
    {
        [Fact]
        public void CreateName_WhenEmptyFirstName_ShouldThrowDomainException()
        {
            Assert.Throws<Exception>(() => new Name("", "Almeida", "Felipe de Almeida"));
        }

        [Fact]
        public void CreateName_WhenEmptyLastName_ShouldThrowDomainException()
        {
            Assert.Throws<Exception>(() => new Name("Felipe", "", "Felipe de Almeida"));
        }

        [Fact]
        public void CreateName_WhenEmptyFullName_ShouldThrowDomainException()
        {
            Assert.Throws<Exception>(() => new Name("Felipe", "Almeida", ""));
        }
    }
}
