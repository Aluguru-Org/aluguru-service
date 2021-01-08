using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Register.Dtos;
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
            When(x => new Regex(@"^(?=.*[A-Z])(?=.*[!@#$&*=-_])(?=.*[0-9])(?=.*[a-z]).{8,32}$").IsMatch(x.Password) == false, () =>
            {
                RuleFor(x => x.Password).Matches(new Regex(@"^.{8,32}$")).WithMessage("The password must have between 8 and 32 characters");
                RuleFor(x => x.Password).Matches(new Regex(@"[A-Z]")).WithMessage("The password must have at least 1 upper character");
                RuleFor(x => x.Password).Matches(new Regex(@"[a-z]")).WithMessage("The password must have at least 1 lower character");
                RuleFor(x => x.Password).Matches(new Regex(@"[0-9]")).WithMessage("The password must have at least 1 numeric character");
                RuleFor(x => x.Password).Matches(new Regex(@"[!@#$&*=-_]")).WithMessage("The password must have at least 1 special character (!@#$&*=-)");
            });
            RuleFor(x => x.Role).Matches(@"company|user", RegexOptions.IgnoreCase).WithMessage("The user role must be Company or User");
        }
    }

    public class CreateUserCommandResponse
    {
        public string ActivationHash { get; set; }
        public UserDTO User { get; set; }
    }
}
