using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Rent.ViewModels
{
    public class ApplyVoucherViewModel : IDto
    {
        public string Code { get; set; }
    }
}
