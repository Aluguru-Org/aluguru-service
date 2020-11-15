using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Rent.Dtos
{
    public class ApplyVoucherDTO : IDto
    {
        public string Code { get; set; }
    }
}
