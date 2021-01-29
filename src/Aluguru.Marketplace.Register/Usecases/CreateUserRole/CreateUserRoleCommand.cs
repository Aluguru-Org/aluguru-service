using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Dtos;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Register.Usecases.CreateUserRole
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
            ValidationResult = new CreateUserRoleCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateUserRoleCommandValidator : AbstractValidator<CreateUserRoleCommand>
    {
        public CreateUserRoleCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(10);
        }
    }

    public class CreateUserRoleCommandResponse
    {
        public UserRoleDTO Role { get; set; }
    }
}
