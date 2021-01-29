using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using FluentValidation;
using System;

namespace Aluguru.Marketplace.Rent.Usecases.ConfirmOrderPayment
{
    public class ConfirmOrderPaymentCommand : Command<bool>
    {
        public ConfirmOrderPaymentCommand(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new ConfirmOrderPaymentCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ConfirmOrderPaymentCommandValidator : AbstractValidator<ConfirmOrderPaymentCommand>
    {
        public ConfirmOrderPaymentCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEqual(Guid.Empty);
        }
    }
}
