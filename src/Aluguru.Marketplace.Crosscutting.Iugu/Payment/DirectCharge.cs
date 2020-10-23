using Aluguru.Marketplace.Crosscutting.Iugu.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Crosscutting.Iugu.Payment
{
    public class DirectCharge
    {
        private DirectCharge() { }
        public DirectCharge(string method, string token, string email, string months, PayerDTO payer, IList<ItemDTO> items)
        {
            Method = method;
            Token = token;
            Email = email;
            Months = months;
            Payer = payer;
            Items = items;
        }

        public string Method { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Months { get; set; }
        public PayerDTO Payer { get; set; }
        public IList<ItemDTO> Items { get; set; }
    } 
}
