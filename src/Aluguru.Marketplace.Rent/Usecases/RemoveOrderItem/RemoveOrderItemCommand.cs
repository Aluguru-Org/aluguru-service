using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Rent.Usecases.RemoveOrderItem
{
    public class RemoveOrderItemCommand : Command<RemoveOrderItemCommandResponse>
    {
        public RemoveOrderItemCommand(Guid userId, Guid orderId, Guid productId)
        {
            UserId = userId;
            OrderId = orderId;
            OrderItemId = productId;
        }

        public Guid UserId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid OrderItemId { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new RemoveOrderItemCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class RemoveOrderItemCommandValidator : AbstractValidator<RemoveOrderItemCommand>
    {
        public RemoveOrderItemCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.OrderId).NotEqual(Guid.Empty);
            RuleFor(x => x.OrderItemId).NotEqual(Guid.Empty);
        }
    }

    public class RemoveOrderItemCommandResponse
    {
        public OrderDTO Order { get; set; }
    }
}
