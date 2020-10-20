using Aluguru.Marketplace.Domain;
using System;

namespace Aluguru.Marketplace.Catalog.ViewModels
{
    public class CreateRentPeriodViewModel : IDto
    {
        public string Name { get; set; }
        public int Days { get; set; }
    }
}