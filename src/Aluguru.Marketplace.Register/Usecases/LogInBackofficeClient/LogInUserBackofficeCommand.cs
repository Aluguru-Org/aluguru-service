using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Register.Usecases.LogInBackofficeClient
{
    public class LogInUserBackofficeCommand : Command<LogInUserBackofficeCommandResponse>
    {
        public LogInUserBackofficeCommand(string username, string password)
        {
            Email = username;
            Password = password;
        }

        public string Email { get; private set; }
        public string Password { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new LogInUserBackofficeCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class LogInUserBackofficeCommandValidator : AbstractValidator<LogInUserBackofficeCommand>
    {
        public LogInUserBackofficeCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class LogInUserBackofficeCommandResponse
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}