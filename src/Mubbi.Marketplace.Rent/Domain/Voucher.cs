using FluentValidation;
using FluentValidation.Results;
using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using static PampaDevs.Utils.Helpers.IdHelper;
using static PampaDevs.Utils.Helpers.DateTimeHelper;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Rent.Domain
{
    public class Voucher : AggregateRoot
    {
        private Voucher() : base(NewId()) { }
        public Voucher(string code, EVoucherType voucherType, decimal discount, int amount, DateTime expirationDate)
            : base(NewId())
        {
            Active = true;
            Used = false;
            CreationDate = NewDateTime();

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

            ValidateEntity();
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

        //Ef Relational
        public List<Order> Orders { get; set; }

        public void Activate() => Active = true;
        public void Deactivate() => Active = false;

        public void MarkAsUsed()
        {
            Used = true;
            Amount -= 1;
            UpdatedDate = NewDateTime();
        }

        public ValidationResult IsValid()
        {            
            return new VoucherApplicableValidation().Validate(this);
        }
        protected override void ValidateEntity()
        {
            Ensure.NotNullOrEmpty(Code, "The field Code cannot be empty");
            if (ValueDiscount != null) Ensure.That(ValueDiscount.Value > 0, "The field ValueDiscount cannot be smaller or equal to 0");
            if (PercentualDiscount != null)
            {
                Ensure.That(PercentualDiscount.Value > 0, "The field PercentualDiscount cannot be smaller or equal to 0");
                Ensure.That(PercentualDiscount.Value <= 100, "The field PercentualDiscount cannot be greater than 100");
            }
            Ensure.That(Amount > 0, "The field Amount cannot be smaller or equal to 0");
            Ensure.That(ExpirationDate > NewDateTime(), "The field ExpirationDate cannot be smaller than the current day");
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