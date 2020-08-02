using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Rent.ViewModels;
using System;

namespace Mubbi.Marketplace.Rent.Usecases.GetOrder
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
        public OrderViewModel Order { get; set; }
    }
}
