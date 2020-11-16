using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Payment.Dtos;
using FluentValidation;
using System;

namespace Aluguru.Marketplace.Payment.Usecases.GetPayment
{
    public class GetPaymentCommand : Command<GetPaymentCommandResponse>
    {
        public GetPaymentCommand(Guid paymentId)
        {
            PaymentId = paymentId;
        }

        public Guid PaymentId { get; set; }
    }

    public class GetPaymentCommandValidator : AbstractValidator<GetPaymentCommand>
    {
        public GetPaymentCommandValidator()
        {
            RuleFor(x => x.PaymentId).NotEqual(Guid.Empty);
        }
    }

    public class GetPaymentCommandResponse
    {
        public PaymentDTO Payment { get; set; }
    }
}
