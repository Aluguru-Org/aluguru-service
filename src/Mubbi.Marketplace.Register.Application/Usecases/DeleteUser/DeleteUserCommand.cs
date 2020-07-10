using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.Usecases.DeleteUser
{
    public class DeleteUserCommand : Command<bool>
    {
        public DeleteUserCommand(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteUserCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }       
    }

    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
        }
    }
}
