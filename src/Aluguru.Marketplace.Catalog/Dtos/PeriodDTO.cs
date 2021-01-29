using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Catalog.Dtos
{
    public class PeriodDTO : IDto
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
