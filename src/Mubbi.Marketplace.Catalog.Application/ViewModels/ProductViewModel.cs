using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Swagger;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mubbi.Marketplace.Catalog.ViewModels
{
    public class ProductViewModel : IDto
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SubCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public decimal Price { get; set; }
        public int MinRentTime { get; set; }
        public int? MaxRentTime { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int StockQuantity { get; set; }
        [SwaggerExclude]
        public CategoryViewModel Category { get; set; }
        [SwaggerExclude]
        public CategoryViewModel SubCategory { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<CustomFieldViewModel> CustomFields { get; set; }
    }
}