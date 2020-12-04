using System;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Catalog.Dtos
{
    public class PriceDTO
    {
        public int FreightPriceKM { get; set; }
        public decimal? SellPrice { get; set; }
        public decimal? DailyRentPrice { get; set; }
        public List<PeriodPriceViewModel> PeriodRentPrices { get; set; }
    }

    public class PeriodPriceViewModel
    {
        public Guid RentPeriodId { get; set; }
        public decimal Price { get; set; }
    }
}
