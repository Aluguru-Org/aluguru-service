using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Register.ViewModels;
using Mubbi.Marketplace.Register.Domain;
using System;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Register.Usecases.GetUsersByRole
{
    public class GetUsersByRoleCommand : Command<GetUsersByRoleCommandResponse>
    {
        public GetUsersByRoleCommand(ERoles role)
        {
            Role = role;
        }

        public ERoles Role { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new GetUsersByTypeCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class GetUsersByTypeCommandValidator : AbstractValidator<GetUsersByRoleCommand>
    {
        public GetUsersByTypeCommandValidator()
        {
            RuleFor(x => x.Role).NotEqual(ERoles.Admin).WithMessage("You do not have access to get admin users");
        }
    }

    public class GetUsersByRoleCommandResponse
    {
        public List<UserViewModel> Users { get; set; }
    }
}
