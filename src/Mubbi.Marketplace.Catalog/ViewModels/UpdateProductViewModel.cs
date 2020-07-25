using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Catalog.ViewModels
{
    public class UpdateProductViewModel : IDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public int StockQuantity { get; set; }
        public int MinRentDays { get; set; }
        public int? MaxRentDays { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<UpdateCustomFieldViewModel> CustomFields { get; set; }
    }
}
