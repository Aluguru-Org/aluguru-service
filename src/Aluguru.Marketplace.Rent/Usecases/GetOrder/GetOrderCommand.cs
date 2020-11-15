using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.Dtos;
using System;

namespace Aluguru.Marketplace.Rent.Usecases.GetOrder
{
    public class GetOrderCommand : Command<GetOrderCommandResponse>
    {
        public GetOrderCommand(Guid orderId)
        {
            OrderId = orderId;
        }
        public Guid OrderId { get; }
        public override bool IsValid()
        {
            ValidationResult = new GetOrderCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class GetOrderCommandValidator : AbstractValidator<GetOrderCommand>
    {
        public GetOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEqual(Guid.Empty);
        }
    }

    public class GetOrderCommandResponse
    {
        public OrderDTO Order { get; set; }
    }
}
