using Aluguru.Marketplace.Domain;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Catalog.Dtos
{
    public class UpdateCustomFieldDTO : IDto
    {
        [SwaggerSchema("The Custom Field type, it can be: 'Text', 'Number', 'Radio' or 'Checkbox'")]
        public string FieldType { get; set; }
        public string FieldName { get; set; }
        public List<string> ValueAsOptions { get; set; }
        [SwaggerSchema("If the Custom Field will appear or not inside the product")]
        public bool Active { get; set; }
    }
}
