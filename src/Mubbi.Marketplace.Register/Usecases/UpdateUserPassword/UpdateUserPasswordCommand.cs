using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Mubbi.Marketplace.Register.Usecases.UpdateUserPassword
{
    public class UpdateUserPasswordCommand : Command<bool>
    {
        public UpdateUserPasswordCommand(Guid currentUserId, Guid userId, string password)
        {
            CurrentUserId = currentUserId;
            UserId = userId;
            Password = password;
        }

        public Guid CurrentUserId { get; set; }
        public Guid UserId { get; set; }
        public string Password { get; set; }
    }

    public class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
    {
        public UpdateUserPasswordCommandValidator()
        {
            RuleFor(x => x.CurrentUserId).NotEqual(Guid.Empty);
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.Password).NotEmpty().WithMessage("The password field cannot be empty");
        }
    }
}
