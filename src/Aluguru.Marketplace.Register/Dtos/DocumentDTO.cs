using Aluguru.Marketplace.Domain;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Aluguru.Marketplace.Register.Dtos
{
    public class DocumentDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Number { get; set; }
        [SwaggerSchema("The Document Type, it can be: 'CNPJ', or 'CPF'")]
        public string DocumentType { get; set; }
    }
}
