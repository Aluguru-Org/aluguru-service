using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mubbi.Marketplace.Catalog.UnitTests
{
    public class RentPeriodTests
    {
        [Fact]
        public void CreateRentPeriod_WhenEmptyName_ShouldThrowException()
        {
            Assert.Throws<DomainException>(() => new RentPeriod("", 30));
        }

        [Fact]
        public void CreateRentPeriod_WhenZeroDays_ShouldThrowException()
        {
            Assert.Throws<DomainException>(() => new RentPeriod("1 month", 0));
        }
    }
}
