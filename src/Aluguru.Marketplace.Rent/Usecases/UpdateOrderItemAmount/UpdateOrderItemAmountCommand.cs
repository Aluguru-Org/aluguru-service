using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.Dtos;
using System;

namespace Aluguru.Marketplace.Rent.Usecases.UpdateOrderItemAmount
{
    public class UpdateOrderItemAmountCommand : Command<UpdateOrderItemAmountCommandResponse>
    {
        public UpdateOrderItemAmountCommand(Guid userId, Guid orderId, Guid orderItemId, int amount)
        {
            UserId = userId;
            OrderId = orderId;
            OrderItemId = orderItemId;
            Amount = amount;
        }

        public Guid UserId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid OrderItemId { get; private set; }
        public int Amount { get; private set; }
        public override bool IsValid()
        {
            ValidationResult = new UpdateOrderItemAmountCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateOrderItemAmountCommandValidator : AbstractValidator<UpdateOrderItemAmountCommand>
    {
        public UpdateOrderItemAmountCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.OrderId).NotEqual(Guid.Empty);
            RuleFor(x => x.OrderItemId).NotEqual(Guid.Empty);
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }

    public class UpdateOrderItemAmountCommandResponse
    {
        public OrderDTO Order { get; set; }
    }
}