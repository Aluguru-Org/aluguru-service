using Aluguru.Marketplace.Domain;
using System;

namespace Aluguru.Marketplace.Rent.Dtos
{
    public class CustomFieldResponseDTO : IDto
    {
        public Guid CustomFieldId { get; set; }
        public string FieldName { get; set; }
        public string[] FieldResponses { get; set; }
    }
}
