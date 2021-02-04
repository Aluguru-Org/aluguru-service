using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.Dtos;
using System;

namespace Aluguru.Marketplace.Rent.Usecases.AddOrderItem
{
    public class AddOrderItemCommand : Command<AddOrderItemCommandResponse>
    {
        public AddOrderItemCommand(Guid userId, Guid orderId, AddOrderItemDTO orderItem)
        {
            UserId = userId;
            OrderId = orderId;
            OrderItem = orderItem;
        }

        public Guid UserId { get; private set; }
        public Guid OrderId { get; private set; }
        public AddOrderItemDTO OrderItem { get; private set; }
        public override bool IsValid()
        {
            ValidationResult = new AddOrderItemCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddOrderItemCommandValidator : AbstractValidator<AddOrderItemCommand>
    {
        public AddOrderItemCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.OrderId).NotEqual(Guid.Empty);
            RuleFor(x => x.OrderItem.ProductId).NotEqual(Guid.Empty);

            When(x => x.OrderItem.RentStartDate.HasValue, () => RuleFor(x => x.OrderItem.RentStartDate).NotEmpty());
        }
    }

    public class AddOrderItemCommandResponse
    {
        public OrderDTO Order { get; set; }
    }
}