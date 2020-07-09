using Mubbi.Marketplace.Domain;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mubbi.Marketplace.Catalog.Application.ViewModels
{
    public class CreateProductViewModel : IDto
    {
        [Required]
        public Guid CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]        
        public decimal Price { get; set; }
        [Required]
        [SwaggerSchema("If the Product will appear on the catalog")]
        public bool IsActive { get; set; }
        [Required]
        public int MinRentDays { get; set; }
        public int? MaxRentDays { get; set; }
        [Required]
        public int StockQuantity { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<CustomFieldViewModel> CustomFields { get; set; }
    }
}
