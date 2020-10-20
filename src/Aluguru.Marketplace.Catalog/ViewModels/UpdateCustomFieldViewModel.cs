using Aluguru.Marketplace.Domain;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Catalog.ViewModels
{
    public class UpdateCustomFieldViewModel : IDto
    {
        [SwaggerSchema("The Custom Field type, it can be: 'Text', 'Number', 'Radio' or 'Checkbox'")]
        public string FieldType { get; set; }
        public string ValueAsString { get; set; }
        public int? ValueAsInt { get; set; }
        public List<string> ValueAsOptions { get; set; }
        [SwaggerSchema("If the Custom Field will appear or not inside the product")]
        public bool Active { get; set; }
    }
}
