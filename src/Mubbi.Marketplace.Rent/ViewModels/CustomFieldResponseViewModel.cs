using Mubbi.Marketplace.Domain;
using System;

namespace Mubbi.Marketplace.Rent.ViewModels
{
    public class CustomFieldResponseViewModel : IDto
    {
        public Guid CustomFieldId { get; set; }
        public Guid FieldName { get; set; }
        public string[] FieldResponses { get; set; }
    }
}
