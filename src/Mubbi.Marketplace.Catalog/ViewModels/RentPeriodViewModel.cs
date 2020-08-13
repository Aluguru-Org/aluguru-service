using Mubbi.Marketplace.Domain;
using System;

namespace Mubbi.Marketplace.Catalog.ViewModels
{
    public class RentPeriodViewModel : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Days { get; set; }
    }
}