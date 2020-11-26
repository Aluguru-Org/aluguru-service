using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Aluguru.Marketplace.Rent.Dtos
{
    public class CreateVoucherDTO
    {
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public int Amount { get; set; }
        [SwaggerSchema("The Voucher Type can be: 'Percent', 'Value'")]
        public string VoucherType { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
