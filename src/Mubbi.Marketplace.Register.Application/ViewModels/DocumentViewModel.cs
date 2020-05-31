using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.Application.ViewModels
{
    public class DocumentViewModel
    {
        public string Number { get; private set; }
        /// <summary>
        /// Document type - CNPJ or CPF
        /// </summary>
        public string DocumentType { get; private set; }
    }
}
