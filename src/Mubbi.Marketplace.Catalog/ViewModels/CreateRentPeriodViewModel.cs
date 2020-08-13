using Mubbi.Marketplace.Domain;
using System;

namespace Mubbi.Marketplace.Catalog.ViewModels
{
    public class CreateRentPeriodViewModel : IDto
    {
        public string Name { get; set; }
        public int Days { get; set; }
    }
}