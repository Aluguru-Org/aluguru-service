using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class DocumentViewModel
    {
        public string Number { get; set; }
        /// <summary>
        /// Document type - CNPJ or CPF
        /// </summary>
        public string DocumentType { get; set; }
    }
}
