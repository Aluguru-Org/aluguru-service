using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Register.Application.ViewModels;
using System.Text.RegularExpressions;

namespace Mubbi.Marketplace.Register.Application.Usecases.CreateUser
{
    public class CreateUserCommand : Command<CreateUserCommandResponse>
    {
        public CreateUserCommand(string fullName, string password, string email, string role, string documentType, string documentNumber, string addressNumber, string addressStreet, string addressNeighborhood, string addressCity, string addressState, string addressCountry, string addressZipCode)
        {
            FullName = fullName;
            Password = password;
            Email = email;
            Role = role;
            DocumentType = documentType;
            DocumentNumber = documentNumber;
            AddressNumber = addressNumber;
            AddressStreet = addressStreet;
            AddressNeighborhood = addressNeighborhood;
            AddressCity = addressCity;
            AddressState = addressState;
            AddressCountry = addressCountry;
            AddressZipCode = addressZipCode;
        }

        public string FullName { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public string DocumentType { get; private set; }
        public string Role { get; private set; }
        public string DocumentNumber { get; private set; }
        public string AddressNumber { get; private set; }
        public string AddressStreet { get; private set; }
        public string AddressNeighborhood { get; private set; }
        public string AddressCity { get; private set; }
        public string AddressState { get; private set; }
        public string AddressCountry { get; private set; }
        public string AddressZipCode { get; private set; }
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
            RuleFor(x => x.DocumentType).Matches(@"cpf|cnpj", RegexOptions.IgnoreCase).WithMessage("The document type must be CPF or CNPJ");
            RuleFor(x => x.DocumentNumber).NotEmpty();
        }
    }

    public class CreateUserCommandResponse
    {
        public UserViewModel User { get; set; }
    }
}
