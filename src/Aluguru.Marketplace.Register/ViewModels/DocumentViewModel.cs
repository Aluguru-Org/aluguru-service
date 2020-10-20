using Aluguru.Marketplace.Domain;
using System;

namespace Aluguru.Marketplace.Register.ViewModels
{
    public class DocumentViewModel : IDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Number { get; set; }
        /// <summary>
        /// Document type - CNPJ or CPF
        /// </summary>
        public string DocumentType { get; set; }
    }
}
