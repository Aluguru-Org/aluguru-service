using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.Dtos;
using System;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Rent.Usecases.CreateOrder
{
    public class CreateOrderCommand : Command<CreateOrderCommandResponse>
    {
        public CreateOrderCommand(Guid userId, List<AddOrderItemDTO> orderItems)
        {
            UserId = userId;
            OrderItems = orderItems;
        }

        public Guid UserId { get; private set; }
        public List<AddOrderItemDTO> OrderItems { get; private set; }
        public override bool IsValid()
        {
            ValidationResult = new CreateOrderCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.OrderItems).NotEmpty().Must(x => 
            {
                return x.TrueForAll(item => item.ProductId != Guid.Empty);
            });
        }
    }

    public class CreateOrderCommandResponse
    {
        public OrderDTO Order { get; set; }
    }
}