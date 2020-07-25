using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Register.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.Usecases.UpdateUserRole
{
    public class UpdateUserRoleCommand : Command<UpdateUserRoleCommandResponse>
    {
        public UpdateUserRoleCommand(Guid userRoleId, UpdateUserRoleViewModel userRole)
        {
            UserRoleId = userRoleId;
            UserRole = userRole;
        }

        public Guid UserRoleId { get; private set; }
        public UpdateUserRoleViewModel UserRole { get; private set; }

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
        public UserRoleViewModel UserRole { get; set; }
    }
}
