using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Register.Usecases.LogInClient
{
    public class LogInUserClientCommand : Command<LogInUserClientCommandResponse>
    {
        public LogInUserClientCommand(string username, string password)
        {
            Email = username;
            Password = password;
        }

        public string Email { get; private set; }
        public string Password { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new LogInUserClientCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class LogInUserClientCommandValidator : AbstractValidator<LogInUserClientCommand>
    {
        public LogInUserClientCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class LogInUserClientCommandResponse
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}