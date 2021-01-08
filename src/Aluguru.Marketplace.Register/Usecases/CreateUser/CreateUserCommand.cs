using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Register.Dtos;
using System.Text.RegularExpressions;
using Aluguru.Marketplace.Register.Domain;

namespace Aluguru.Marketplace.Register.Usecases.CreateUser
{
    public class CreateUserCommand : Command<CreateUserCommandResponse>
    {
        public CreateUserCommand(string fullName, string password, string email, string role, Document document, Address address, Contact contact)
        {
            FullName = fullName;
            Password = password;
            Email = email;
            Role = role;
            Document = document;
            Address = address;
            Contact = contact;
        }

        public string FullName { get; }
        public string Password { get; }
        public string Email { get; }
        public string Role { get; }   
        public Document Document { get; }
        public Address Address { get; }
        public Contact Contact { get; }
        

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

            When(x => x.Address != null, () =>
            {
                RuleFor(x => x.Address.ZipCode).NotEmpty();
                RuleFor(x => x.Address.Street).NotEmpty();
                RuleFor(x => x.Address.Number).NotEmpty();
                RuleFor(x => x.Address.Neighborhood).NotEmpty();
                RuleFor(x => x.Address.City).NotEmpty();
                RuleFor(x => x.Address.State).NotEmpty();
                RuleFor(x => x.Address.Country).NotEmpty();
            });

            When(x => x.Document != null, () =>
            {
                RuleFor(x => x.Document.DocumentType).Must(x => x == EDocumentType.CNPJ || x == EDocumentType.CPF)
                    .WithMessage("DocumentType must be CNPJ or CPF");
                RuleFor(x => x.Document.Number).NotEmpty();
            });

            When(x => x.Contact != null, () =>
            {
                RuleFor(x => x.Contact.Name).NotEmpty();
                RuleFor(x => x.Contact.PhoneNumber).NotEmpty();
                RuleFor(x => x.Contact.Email).NotEmpty();
            });
        }
    }

    public class CreateUserCommandResponse
    {
        public string ActivationHash { get; set; }
        public UserDTO User { get; set; }
    }
}
