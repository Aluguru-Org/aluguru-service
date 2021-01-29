using Aluguru.Marketplace.Domain;
using System;

namespace Aluguru.Marketplace.Catalog.Dtos
{
    public class CreateRentPeriodDTO : IDto
    {
        public string Name { get; set; }
        public int Days { get; set; }
    }
}