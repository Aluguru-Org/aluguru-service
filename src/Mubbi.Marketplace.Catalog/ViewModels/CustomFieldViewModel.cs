using Mubbi.Marketplace.Domain;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Catalog.ViewModels
{
    public class CustomFieldViewModel : IDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        [SwaggerSchema("The Custom Field type, it can be: 'Text', 'Number', 'Radio' or 'Checkbox'")]
        public string FieldType { get; set; }
        public string ValueAsString { get; set; }
        public int? ValueAsInt { get; set; }
        public List<string> ValueAsOptions { get; set; }
        [SwaggerSchema("If the Custom Field will appear or not inside the product")]
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}