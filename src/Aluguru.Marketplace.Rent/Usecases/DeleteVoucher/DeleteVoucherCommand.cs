using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Rent.Usecases.DeleteVoucher
{
    public class DeleteVoucherCommand : Command<bool>
    {
        public DeleteVoucherCommand(string code)
        {
            Code = code;
        }

        public string Code { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteVoucherCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }       
    }

    public class DeleteVoucherCommandValidator : AbstractValidator<DeleteVoucherCommand>
    {
        public DeleteVoucherCommandValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
