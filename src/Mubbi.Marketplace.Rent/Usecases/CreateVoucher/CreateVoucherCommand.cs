using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Rent.Domain;
using Mubbi.Marketplace.Rent.ViewModels;
using System;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Rent.Usecases.CreateVoucher
{
    public class CreateVoucherCommand : Command<CreateVoucherCommandResponse>
    {
        public CreateVoucherCommand(CreateVoucherViewModel voucher)
        {
            Voucher = voucher;
        }

        public CreateVoucherViewModel Voucher { get; private set; }
        public override bool IsValid()
        {
            ValidationResult = new CreateVoucherCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateVoucherCommandValidator : AbstractValidator<CreateVoucherCommand>
    {
        public CreateVoucherCommandValidator()
        {
            RuleFor(x => x.Voucher.Code).NotEmpty();
            RuleFor(x => x.Voucher.Amount).GreaterThan(0);
            RuleFor(x => x.Voucher.VoucherType);
            RuleFor(x => x.Voucher.Discount).GreaterThan(0);
            RuleFor(x => x.Voucher.ExpirationDate).NotEqual(DateTime.MinValue);
        }
    }

    public class CreateVoucherCommandResponse
    {
        public VoucherViewModel Voucher { get; set; }
    }
}