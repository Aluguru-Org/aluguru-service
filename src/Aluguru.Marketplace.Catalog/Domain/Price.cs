using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aluguru.Marketplace.Catalog.Domain
{
    public class Price : ValueObject
    {
        public Price(decimal? sellPrice, decimal? dailyRentPrice, List<PeriodPrice> periodRentPrices)
        {
            SellPrice = sellPrice;
            DailyRentPrice = dailyRentPrice;
            PeriodRentPrices = periodRentPrices;
            ValidateValueObject();
        }

        public decimal? SellPrice { get; private set; }
        public decimal? DailyRentPrice { get; private set; }
        public List<PeriodPrice> PeriodRentPrices { get; private set; }

        public decimal GetSellPrice()
        {
            return SellPrice ?? 0;
        }

        public decimal GetDailyRentPrice()
        {
            return DailyRentPrice ?? 0;
        }

        public decimal GetPeriodRentPrice(Guid rentPeriodId)
        {
            var periodPrice = PeriodRentPrices.FirstOrDefault(x => x.RentPeriodId == rentPeriodId);

            return periodPrice?.GetPrice() ?? 0;
        }

        public void UpdateSellPrice(decimal? price)
        {
            if (price.HasValue)
            {
                Ensure.That<DomainException>(price > 0, "The field SellPrice from Product cannot be smaller or equal than zero");
            }
            SellPrice = price;
        }

        public void UpdateDailyRentPrice(decimal? price)
        {
            if (price.HasValue)
            {
                Ensure.That<DomainException>(price > 0, "The field DailyRentPrice from Product cannot be smaller or equal than zero");
            }
            DailyRentPrice = price;
        }

        public void UpdatePeriodRentPrices(List<PeriodPriceViewModel> periodPrices)
        {
            if (periodPrices != null && periodPrices.Count > 0)
            {
                Ensure.That<DomainException>(periodPrices.All(x => x.Price > 0), "The field PeriodRentPrices from Product cannot have a price that is smaller or equal than zero");
            }
            PeriodRentPrices.Clear();

            PeriodRentPrices = periodPrices.Select(x => new PeriodPrice(x.RentPeriodId, x.Price)).ToList();
        }

        protected override void ValidateValueObject()
        {
            Ensure.That<DomainException>(SellPrice != null || DailyRentPrice != null || PeriodRentPrices != null, "Price cannot be empty");

            if (SellPrice.HasValue) Ensure.That<DomainException>(SellPrice.Value > 0, "The sell price cannot be less than one");
            if (DailyRentPrice.HasValue) Ensure.That<DomainException>(DailyRentPrice.Value > 0, "The daily rent price cannot be less than one");
            if (PeriodRentPrices != null && PeriodRentPrices.Count > 0)
            {
                foreach (var periodPrice in PeriodRentPrices)
                {
                    Ensure.That<DomainException>(periodPrice.Price > 0, $"The period rent price from [{periodPrice.RentPeriodId}] cannot be less than one");
                }
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return SellPrice;
            yield return DailyRentPrice;
            yield return PeriodRentPrices;
        }
    }

    public class PeriodPrice 
    {
        public PeriodPrice(Guid rentPeriodId, decimal price)
        {
            RentPeriodId = rentPeriodId;
            Price = price;
        }

        public Guid RentPeriodId { get; private set; }
        public decimal Price { get; private set; }

        public decimal GetPrice()
        {
            return Price;
        }
    }
}
