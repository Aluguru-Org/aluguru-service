using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Register.Dtos;
using System;
using Aluguru.Marketplace.Register.Domain;

namespace Aluguru.Marketplace.Register.Usecases.UpadeUserContact
{
    public class UpdateUserContactCommand : Command<bool>
    {
        public UpdateUserContactCommand(Guid userId, string name, string phoneNumber, string email)
        {
            UserId = userId;
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateUserCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserContactCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}
