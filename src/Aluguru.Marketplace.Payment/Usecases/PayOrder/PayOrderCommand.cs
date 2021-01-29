using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Payment.Dtos;
using FluentValidation;
using System;

namespace Aluguru.Marketplace.Payment.Usecases.PayOrder
{
    public class PayOrderCommand : Command<PayOrderCommandResponse>
    {
        public PayOrderCommand(Guid userId, Guid orderId, int? installments, string token)
        {
            OrderId = orderId;
            Installments = installments;
            Token = token;
            UserId = userId;
        }
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public int? Installments { get; set; }
        public string Token { get; set; }
    }

    public class PayOrderCommandValidator : AbstractValidator<PayOrderCommand>
    {
        public PayOrderCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.OrderId).NotEqual(Guid.Empty);
            When(x => x.Installments.HasValue, () =>
            {
                RuleFor(x => x.Installments.Value).GreaterThan(0);
                RuleFor(x => x.Installments.Value).LessThanOrEqualTo(12);
            });
            RuleFor(x => x.Token).NotEmpty();
        }
    }

    public class PayOrderCommandResponse
    {
        public PaymentDTO Payment { get; set; }
    }
}
