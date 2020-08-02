﻿using Mubbi.Marketplace.Domain;
using System;

namespace Mubbi.Marketplace.Rent.ViewModels
{
    public class VoucherViewModel : IDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public decimal? PercentualDiscount { get; set; }
        public decimal? ValueDiscount { get; set; }
        public int Amount { get; set; }
        public string VoucherType { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Active { get; set; }
        public bool Used { get; set; }
    }
}
