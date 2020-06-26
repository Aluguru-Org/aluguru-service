using FluentValidation;
using Mubbi.Marketplace.Shared.Messages;
using System.Security.Cryptography.X509Certificates;

namespace Mubbi.Marketplace.Register.Application.Usecases.LogInUser
{
    public class LogInUserCommand : Command<LogInUserCommandResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }

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
