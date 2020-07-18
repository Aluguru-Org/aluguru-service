using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Register.Domain;
using Mubbi.Marketplace.Register.ViewModels;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Register.Usecases.CreateRole
{
    public class CreateRoleCommand : Command<CreateRoleCommandResponse>
    {
        public CreateRoleCommand(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public override bool IsValid()
        {
            ValidationResult = new CreateRoleCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(10);
        }
    }

    public class CreateRoleCommandResponse
    {
        public UserRoleViewModel Role { get; set; }
    }
}
