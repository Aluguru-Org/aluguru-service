using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Register.Domain;
using Mubbi.Marketplace.Register.ViewModels;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Register.Usecases.CreateUserRole
{
    public class CreateUserRoleCommand : Command<CreateUserRoleCommandResponse>
    {
        public CreateUserRoleCommand(string name, List<UserClaim> userClaims)
        {
            Name = name;
            UserClaims = userClaims;
        }

        public string Name { get; private set; }
        public List<UserClaim> UserClaims { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new CreateRoleCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateRoleCommandValidator : AbstractValidator<CreateUserRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(10);
        }
    }

    public class CreateUserRoleCommandResponse
    {
        public UserRoleViewModel Role { get; set; }
    }
}
