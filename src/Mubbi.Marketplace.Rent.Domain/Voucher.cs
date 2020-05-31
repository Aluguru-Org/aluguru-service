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
        public Voucher(string code, EVoucherType voucherType, decimal discount, int amount, DateTime expirationDate)
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

            ValidateCreation();
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
            EntityConcerns.IsEmpty(Code, "The field Code cannot be empty");
            if (ValueDiscount != null) EntityConcerns.SmallerOrEqualThan(0, ValueDiscount.Value, "The field ValueDiscount cannot be smaller or equal to 0");
            if (PercentualDiscount != null)
            {
                EntityConcerns.SmallerOrEqualThan(0, PercentualDiscount.Value, "The field PercentualDiscount cannot be smaller or equal to 0");
                EntityConcerns.GreaterThan(100, PercentualDiscount.Value, "The field PercentualDiscount cannot be greater than 100");
            }
            EntityConcerns.SmallerOrEqualThan(0, Amount, "The field Amount cannot be smaller or equal to 0");
            EntityConcerns.SmallerOrEqualThan(DateTime.UtcNow, ExpirationDate, "The field ExpirationDate cannot be smaller than the current day");
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