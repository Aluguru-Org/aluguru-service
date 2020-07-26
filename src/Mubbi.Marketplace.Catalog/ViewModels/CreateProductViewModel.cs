using Mubbi.Marketplace.Domain;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mubbi.Marketplace.Catalog.ViewModels
{
    public class CreateProductViewModel : IDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [SwaggerSchema("The rent type. It can be 'Indefinite' or 'Fixed'")]
        public string RentType { get; set; }
        [Required]        
        public decimal Price { get; set; }
        [Required]
        [SwaggerSchema("If the Product will appear on the catalog")]
        public bool IsActive { get; set; }
        [Required]
        public int MinNoticeRentDays { get; set; }
        [Required]
        public int MinRentDays { get; set; }
        public int? MaxRentDays { get; set; }
        [Required]
        public int StockQuantity { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<CreateCustomFieldViewModel> CustomFields { get; set; }
    }
}
