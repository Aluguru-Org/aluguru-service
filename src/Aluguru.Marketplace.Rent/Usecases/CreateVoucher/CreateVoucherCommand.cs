using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.Domain;
using Aluguru.Marketplace.Rent.Dtos;
using System;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Rent.Usecases.CreateVoucher
{
    public class CreateVoucherCommand : Command<CreateVoucherCommandResponse>
    {
        public CreateVoucherCommand(CreateVoucherDTO voucher)
        {
            Voucher = voucher;
        }

        public CreateVoucherDTO Voucher { get; private set; }
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
        public VoucherDTO Voucher { get; set; }
    }
}