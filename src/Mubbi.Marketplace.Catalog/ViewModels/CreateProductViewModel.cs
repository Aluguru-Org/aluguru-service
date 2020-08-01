using Mubbi.Marketplace.Domain;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mubbi.Marketplace.Catalog.ViewModels
{
    public class CreateProductViewModel : IDto
    {
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [SwaggerSchema("The rent type. It can be 'Indefinite' or 'Fixed'")]
        public string RentType { get; set; }
        public decimal Price { get; set; }
        [SwaggerSchema("If the Product will appear on the catalog")]
        public bool IsActive { get; set; }
        public int MinRentDays { get; set; }
        public int? MinNoticeRentDays { get; set; }
        public int? MaxRentDays { get; set; }
        public int StockQuantity { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<CreateCustomFieldViewModel> CustomFields { get; set; }
    }
}
