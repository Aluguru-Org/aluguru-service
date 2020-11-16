using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.Dtos;
using FluentValidation;
using System;

namespace Aluguru.Marketplace.Rent.Usecases.StartOrder
{
    public class StartOrderCommand : Command<StartOrderCommandResponse>
    {
        public StartOrderCommand(Guid userId, Guid orderId)
        {
            UserId = userId;
            OrderId = orderId;
        }

        public Guid UserId { get; private set; }
        public Guid OrderId { get; private set; }
    }

    public class CreateOrderCommandValidator : AbstractValidator<StartOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.OrderId).NotEqual(Guid.Empty);
        }
    }

    public class StartOrderCommandResponse
    {
        public OrderDTO Order { get; set; }
    }
}
