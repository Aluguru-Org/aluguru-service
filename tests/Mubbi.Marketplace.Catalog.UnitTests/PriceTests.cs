using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Mubbi.Marketplace.Catalog.UnitTests
{
    public class PriceTests
    {
        [Fact]
        public void CreatePrice_WhenInvalid_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Price(null, null, null));
        }

        [Fact]
        public void CreatePrice_WhenSellPriceIsZero_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Price(0, 50000, null));
        }

        [Fact]
        public void CreatePrice_WhenDailyRentPriceIsZero_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Price(null, 0, null));
        }

        [Fact]
        public void CreatePrice_WhenAnyPeriodRentPriceIsZero_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Price(null, null, new List<PeriodPrice>
            {
                new PeriodPrice(NewId(), 0)
            }));
        }
    }
}
