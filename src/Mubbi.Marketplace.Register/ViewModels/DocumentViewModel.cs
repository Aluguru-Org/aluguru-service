using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.ViewModels
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
