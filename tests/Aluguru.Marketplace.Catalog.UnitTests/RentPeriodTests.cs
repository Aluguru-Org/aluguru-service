using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Aluguru.Marketplace.Catalog.UnitTests
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
