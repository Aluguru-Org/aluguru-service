using Aluguru.Marketplace.Domain;
using System;

namespace Aluguru.Marketplace.Catalog.Dtos
{
    public class RentPeriodDTO : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Days { get; set; }
    }
}