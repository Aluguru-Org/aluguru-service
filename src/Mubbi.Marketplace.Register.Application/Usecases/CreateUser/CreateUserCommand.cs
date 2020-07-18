using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Register.ViewModels;
using System.Text.RegularExpressions;

namespace Mubbi.Marketplace.Register.Usecases.CreateUser
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

        public string FullName { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public string Role { get; private set; }

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
            RuleFor(x => x.FullName).NotEmpty().MinimumLength(1);
            RuleFor(x => x.Password).NotEmpty().WithMessage("The password field cannot be empty");
            RuleFor(x => x.Role).Matches(@"company|user", RegexOptions.IgnoreCase).WithMessage("The user role must be Company or User");
        }
    }

    public class CreateUserCommandResponse
    {
        public UserViewModel User { get; set; }
    }
}
