using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Register.ViewModels;
using Aluguru.Marketplace.Register.Domain;
using System;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Register.Usecases.GetUsersByRole
{
    public class GetUsersByRoleCommand : Command<GetUsersByRoleCommandResponse>
    {
        public GetUsersByRoleCommand(string role)
        {
            Role = role;
        }

        public string Role { get; private set; }

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
            RuleFor(x => x.Role).Must(x => AluguruRoles.Contains(x)).WithMessage("The informed role does not exist");
        }
    }

    public class GetUsersByRoleCommandResponse
    {
        public List<UserViewModel> Users { get; set; }
    }
}
