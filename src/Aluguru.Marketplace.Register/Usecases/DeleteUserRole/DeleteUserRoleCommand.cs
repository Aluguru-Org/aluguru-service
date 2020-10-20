using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Register.Usecases.DeleteUserRole
{
    public class DeleteUserRoleCommand : Command<bool>
    {
        public DeleteUserRoleCommand(Guid userRoleId)
        {
            UserRoleId = userRoleId;
        }

        public Guid UserRoleId { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteUserCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserRoleCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.UserRoleId).NotEqual(Guid.Empty);
        }
    }
}