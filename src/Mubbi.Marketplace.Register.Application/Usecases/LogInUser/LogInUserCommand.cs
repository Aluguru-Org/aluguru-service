using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;

namespace Mubbi.Marketplace.Register.Application.Usecases.LogInUser
{
    public class LogInUserCommand : Command<LogInUserCommandResponse>
    {
        public LogInUserCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; private set; }
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
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
