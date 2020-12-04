using Aluguru.Marketplace.Domain;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Aluguru.Marketplace.Register.Dtos
{
    public class UpdateUserDocumentDTO : IDto
    {
        public string Number { get; set; }
        [SwaggerSchema("'CPF' or 'CNPJ'")]
        public string DocumentType { get; set; }
    }
}