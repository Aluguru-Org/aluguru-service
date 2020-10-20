using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.ViewModels;
using System;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Rent.Usecases.ApplyVoucher
{
    public class ApplyVoucherCommand : Command<ApplyVoucherCommandResponse>
    {
        public ApplyVoucherCommand(Guid orderId, string voucherCode)
        {
            OrderId = orderId;
            VoucherCode = voucherCode;
        }

        public Guid OrderId { get; private set; }
        public string VoucherCode { get; private set; }
        public override bool IsValid()
        {
            ValidationResult = new CreateOrderCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateOrderCommandValidator : AbstractValidator<ApplyVoucherCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEqual(Guid.Empty);
            RuleFor(x => x.VoucherCode).NotEmpty();
        }
    }

    public class ApplyVoucherCommandResponse
    {
        public OrderViewModel Order { get; set; }
    }
}