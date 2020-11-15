using System.Collections.Generic;

namespace Aluguru.Marketplace.Crosscutting.Iugu.Dtos
{
    public class DirectChargeDTO
    {
        public string Method { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Months { get; set; }
        public PayerDTO Payer { get; set; }
        public IList<ItemDTO> Items { get; set; }
    } 
}
