using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Register.ViewModels;
using System.Text.RegularExpressions;

namespace Aluguru.Marketplace.Register.Usecases.CreateUser
{
    public class CreateUserCommand : Command<CreateUserCommandResponse>
    {
        public CreateUserCommand(string fullName, string password, string email, string role)
        {
            FullName = fullName;
            Password = password;
            Email = email;
            Role = role;
        }

        public string FullName { get; }
        public string Password { get; }
        public string Email { get; }
        public string Role { get; }

        public override bool IsValid()
        {
            ValidationResult = new CreateUserCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.FullName).NotEmpty().MinimumLength(2);
            RuleFor(x => x.Password).NotEmpty().WithMessage("The password field cannot be empty");
            RuleFor(x => x.Role).Matches(@"company|user", RegexOptions.IgnoreCase).WithMessage("The user role must be Company or User");
        }
    }

    public class CreateUserCommandResponse
    {
        public UserViewModel User { get; set; }
    }
}
