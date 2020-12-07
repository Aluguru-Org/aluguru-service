using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aluguru.Marketplace.Rent.Dtos
{
    public class ApplyVoucherDTO : IDto
    {
        [Required]
        public string Code { get; set; }
    }
}
