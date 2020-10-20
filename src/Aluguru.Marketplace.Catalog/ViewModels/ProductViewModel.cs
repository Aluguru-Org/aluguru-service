using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Swagger;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aluguru.Marketplace.Catalog.ViewModels
{
    public class ProductViewModel : IDto
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RentType { get; set; }
        public bool IsActive { get; set; }
        public PriceViewModel Price { get; set; }
        public int MinRentDays { get; set; }
        public int? MaxRentDays { get; set; }
        public int? MinNoticeRentDays { get; set; }
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