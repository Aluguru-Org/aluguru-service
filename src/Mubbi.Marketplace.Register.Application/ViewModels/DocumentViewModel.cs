using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.Application.ViewModels
{
    public class DocumentViewModel
    {
        public string Number { get; set; }
        /// <summary>
        /// Document type - CNPJ or CPF
        /// </summary>
        public string Type { get; set; }
    }
}
