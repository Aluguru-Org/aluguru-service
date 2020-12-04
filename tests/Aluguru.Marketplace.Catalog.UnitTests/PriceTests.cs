using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using System.Collections.Generic;
using Xunit;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Aluguru.Marketplace.Catalog.UnitTests
{
    public class PriceTests
    {
        [Fact]
        public void CreatePrice_WhenInvalid_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Price(0, null, null, null));
        }

        [Fact]
        public void CreatePrice_WhenFreightPriceKMIsZero_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Price(0, null, 50000, null));
        }

        [Fact]
        public void CreatePrice_WhenSellPriceIsZero_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Price(500, 0, 50000, null));
        }

        [Fact]
        public void CreatePrice_WhenDailyRentPriceIsZero_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Price(500, null, 0, null));
        }

        [Fact]
        public void CreatePrice_WhenAnyPeriodRentPriceIsZero_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Price(500, null, null, new List<PeriodPrice>
            {
                new PeriodPrice(NewId(), 0)
            }));
        }
    }
}
