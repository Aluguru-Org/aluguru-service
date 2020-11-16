using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Register.Usecases.LogInUser
{
    public class LogInUserCommand : Command<LogInUserCommandResponse>
    {
        public LogInUserCommand(string username, string password)
        {
            Email = username;
            Password = password;
        }

        public string Email { get; private set; }
        public string Password { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new LogInUserCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class LogInUserCommandValidator : AbstractValidator<LogInUserCommand>
    {
        public LogInUserCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class LogInUserCommandResponse
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}