using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Aluguru.Marketplace.Rent.Dtos
{
    public class CreateVoucherDTO
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public decimal Discount { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        [SwaggerSchema("The Voucher Type can be: 'Percent', 'Value'")]
        public string VoucherType { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
    }
}
