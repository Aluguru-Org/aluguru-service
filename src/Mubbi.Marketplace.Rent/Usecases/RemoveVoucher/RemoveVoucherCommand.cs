using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Rent.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Rent.Usecases.RemoveVoucher
{
    public class RemoveVoucherCommand : Command<DeleteVoucherCommandResponse>
    {
        public RemoveVoucherCommand(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteVoucherCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }       
    }

    public class DeleteVoucherCommandValidator : AbstractValidator<RemoveVoucherCommand>
    {
        public DeleteVoucherCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEqual(Guid.Empty);
        }
    }

    public class DeleteVoucherCommandResponse
    {
        public OrderViewModel Order { get; set; }
    }
}
