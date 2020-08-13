using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Catalog.Usecases.DeleteRentPeriod
{
    public class DeleteRentPeriodCommand : Command<bool>
    {
        public DeleteRentPeriodCommand(Guid rentPeriodId)
        {
            RentPeriodId = rentPeriodId;
        }

        public Guid RentPeriodId { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteRentPeriodCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class DeleteRentPeriodCommandValidator : AbstractValidator<DeleteRentPeriodCommand>
    {
        public DeleteRentPeriodCommandValidator()
        {
            RuleFor(x => x.RentPeriodId).NotEqual(Guid.Empty);
        }
    }
}
