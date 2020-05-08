using Mubbi.Marketplace.Register.Domain.ValueObjects;
using Mubbi.Marketplace.Shared.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mubbi.Marketplace.Register.Domain.Tests
{
    public class NameTests
    {
        [Fact]
        public void CreateName_WhenEmptyFirstName_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Name("", "Almeida"));
        }

        [Fact]
        public void CreateName_WhenEmptyLastName_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Name("Felipe", ""));
        }
    }
}
