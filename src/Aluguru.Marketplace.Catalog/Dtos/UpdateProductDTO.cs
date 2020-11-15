using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Catalog.Dtos
{
    public class UpdateProductDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public string Description { get; set; }
        [SwaggerSchema("The rent type. It can be 'Indefinite' or 'Fixed'")]
        public string RentType { get; set; }
        public PriceDTO Price { get; set; }
        public bool IsActive { get; set; }
        public int StockQuantity { get; set; }
        public int MinRentDays { get; set; }
        public int? MaxRentDays { get; set; }
        public int? MinNoticeRentDays { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<UpdateCustomFieldDTO> CustomFields { get; set; }
    }
}
