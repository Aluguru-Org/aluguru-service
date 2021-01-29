using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using FluentValidation;
using System;

namespace Aluguru.Marketplace.Register.Usecases.ActivateUser
{
    public class ActivateUserCommand : Command<bool>
    {
        public ActivateUserCommand(Guid userId, string activationHash)
        {
            UserId = userId;
            ActivationHash = activationHash;
        }

        public Guid UserId { get; set; }
        public string ActivationHash { get; set; }
    }

    public class ActivateUserCommandValidator : AbstractValidator<ActivateUserCommand>
    {
        public ActivateUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty).WithMessage("The UserId field cannot be empty");
            RuleFor(x => x.ActivationHash).NotEmpty().WithMessage("The ActivationHash field cannot be empty");
        }
    }
}
