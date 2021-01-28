using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Swagger;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aluguru.Marketplace.Catalog.Dtos
{
    public class ProductDTO : IDto
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public string Description { get; set; }
        public string RentType { get; set; }
        public bool IsActive { get; set; }
        public PriceDTO Price { get; set; }
        public int MinRentDays { get; set; }
        public int? MaxRentDays { get; set; }
        public int? MinNoticeRentDays { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int StockQuantity { get; set; }
        [SwaggerExclude]
        public CategoryDTO Category { get; set; }
        [SwaggerExclude]
        public CategoryDTO SubCategory { get; set; }
        public List<string> ImageUrls { get; set; }
        public InvalidDatesDTO InvalidDates { get; set; }
        public List<CustomFieldDTO> CustomFields { get; set; }
    }
}