using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Register.ViewModels;
using System;

namespace Aluguru.Marketplace.Register.Usecases.UpadeUser
{
    public class UpdateUserCommand : Command<UpdateUserCommandResponse>
    {
        public UpdateUserCommand(Guid userId, string fullName, DocumentViewModel document, ContactViewModel contact, AddressViewModel address)
        {
            UserId = userId;
            FullName = fullName;
            Document = document;
            Contact = contact;
            Address = address;
        }
        public Guid UserId { get; }
        public string FullName { get; }
        public DocumentViewModel Document { get; }
        public ContactViewModel Contact { get; }
        public AddressViewModel Address { get; }

        public override bool IsValid()
        {
            return base.IsValid();
        }
    }

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            When(x => !string.IsNullOrEmpty(x.FullName), () =>
            {
                RuleFor(x => x.FullName).NotEmpty().MinimumLength(2);
            });
        }
    }

    public class UpdateUserCommandResponse
    {
        public UserViewModel User { get; set; }
    }
}
