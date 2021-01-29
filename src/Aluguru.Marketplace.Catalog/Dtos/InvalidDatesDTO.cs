using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Catalog.Dtos
{
    public class InvalidDatesDTO : IDto
    {
        public List<DayOfWeek> Days { get; set; }
        public List<DateTime> Dates { get; set; }
        public List<PeriodDTO> Periods { get; set; }
    }
}
