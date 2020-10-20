using Aluguru.Marketplace.Domain;
using System;

namespace Aluguru.Marketplace.Rent.ViewModels
{
    public class CustomFieldResponseViewModel : IDto
    {
        public Guid CustomFieldId { get; set; }
        public string FieldName { get; set; }
        public string[] FieldResponses { get; set; }
    }
}
