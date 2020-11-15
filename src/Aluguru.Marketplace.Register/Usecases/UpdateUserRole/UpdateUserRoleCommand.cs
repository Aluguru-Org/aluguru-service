using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Register.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Register.Usecases.UpdateUserRole
{
    public class UpdateUserRoleCommand : Command<UpdateUserRoleCommandResponse>
    {
        public UpdateUserRoleCommand(Guid userRoleId, UpdateUserRoleDTO userRole)
        {
            UserRoleId = userRoleId;
            UserRole = userRole;
        }

        public Guid UserRoleId { get; private set; }
        public UpdateUserRoleDTO UserRole { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateUserRoleCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
    {
        public UpdateUserRoleCommandValidator()
        {
            RuleFor(x => x.UserRole.Id).NotEqual(Guid.Empty);
            RuleFor(x => x.UserRole.Name).NotEmpty().MinimumLength(3).MaximumLength(10);
        }
    }

    public class UpdateUserRoleCommandResponse
    {
        public UserRoleDTO UserRole { get; set; }
    }
}
