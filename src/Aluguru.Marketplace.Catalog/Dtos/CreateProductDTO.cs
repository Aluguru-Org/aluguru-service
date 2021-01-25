using Aluguru.Marketplace.Domain;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aluguru.Marketplace.Catalog.Dtos
{
    public class CreateProductDTO : IDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Uri { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [SwaggerSchema("The rent type. It can be 'Indefinite' or 'Fixed'")]
        public string RentType { get; set; }
        [Required]
        public PriceDTO Price { get; set; }
        [Required]
        [SwaggerSchema("If the Product will appear on the catalog")]
        public bool IsActive { get; set; }
        [Required]
        public int MinRentDays { get; set; }
        [SwaggerSchema("The minimum amount of days to be able to deliver the product")]
        public int? MinNoticeRentDays { get; set; }
        public int? MaxRentDays { get; set; }
        [Required]
        public int StockQuantity { get; set; }
        public List<DateTime> BlockedDates { get; set; }
        public List<CreateCustomFieldDTO> CustomFields { get; set; }
    }
}
