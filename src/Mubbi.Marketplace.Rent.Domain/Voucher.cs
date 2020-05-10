using FluentValidation;
using FluentValidation.Results;
using Mubbi.Marketplace.Shared.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Rent.Domain
{
    public class Voucher : Entity
    {
        public Voucher(string code, EVoucherType voucherType, decimal? discount, int amount, DateTime expirationDate)
        {
            Active = true;
            Used = false;
            CreationDate = DateTime.UtcNow;

            VoucherType = voucherType;

            if (VoucherType == EVoucherType.Percent)
            {
                PercentualDiscount = discount;
            }
            else
            {
                ValueDiscount = discount;
            }

            Code = code;
            Amount = amount;

            ExpirationDate = expirationDate;
        }

        public string Code { get; private set; }
        public decimal? PercentualDiscount { get; private set; }
        public decimal? ValueDiscount { get; private set; }
        public int Amount { get; private set; }
        public EVoucherType VoucherType { get; private set; }

        public DateTime CreationDate { get; private set; }
        public DateTime? UpdatedDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool Used { get; private set; }

        internal ValidationResult IsValid()
        {
            return new VoucherApplicableValidation().Validate(this);
        }
        public override void ValidateCreation()
        {
            throw new NotImplementedException();
        }

    }

    public class VoucherApplicableValidation : AbstractValidator<Voucher>
    {
        public VoucherApplicableValidation()
        {
            RuleFor(x => x.ExpirationDate)
                .Must((expirationDate) =>  expirationDate >= DateTime.UtcNow)
                .WithMessage("Expired voucher");

            RuleFor(x => x.Active)
                .Equal(true)
                .WithMessage("This voucher is no longer valid");

            RuleFor(x => x.Used)
                .Equal(false)
                .WithMessage("This voucher has already been used");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("This voucher is no longer avaiable");
        }
    }
}
