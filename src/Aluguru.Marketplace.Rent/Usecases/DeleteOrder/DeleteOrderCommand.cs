using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Rent.Usecases.DeleteOrder
{
    public class DeleteOrderCommand : Command<bool>
    {
        public DeleteOrderCommand(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteUserCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }       
    }

    public class DeleteUserCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEqual(Guid.Empty);
        }
    }
}
