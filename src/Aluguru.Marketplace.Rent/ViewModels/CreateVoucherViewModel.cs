using System;

namespace Aluguru.Marketplace.Rent.ViewModels
{
    public class CreateVoucherViewModel
    {
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public int Amount { get; set; }
        public string VoucherType { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
