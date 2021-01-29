using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Register.Usecases.UpadeUserName
{
    public class UpdateUserNameCommand : Command<bool>
    {
        public UpdateUserNameCommand(Guid userId, string fullName)
        {
            UserId = userId;
            FullName = fullName;
        }
        public Guid UserId { get; }
        public string FullName { get; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateUserCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserNameCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.FullName).NotEmpty().MinimumLength(2);
        }
    }
}
