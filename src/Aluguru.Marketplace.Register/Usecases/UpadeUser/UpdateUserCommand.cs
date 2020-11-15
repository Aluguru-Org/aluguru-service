using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Register.Dtos;
using System;

namespace Aluguru.Marketplace.Register.Usecases.UpadeUser
{
    public class UpdateUserCommand : Command<UpdateUserCommandResponse>
    {
        public UpdateUserCommand(Guid userId, string fullName, DocumentDTO document, ContactDTO contact, AddressDTO address)
        {
            UserId = userId;
            FullName = fullName;
            Document = document;
            Contact = contact;
            Address = address;
        }
        public Guid UserId { get; }
        public string FullName { get; }
        public DocumentDTO Document { get; }
        public ContactDTO Contact { get; }
        public AddressDTO Address { get; }

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
        public UserDTO User { get; set; }
    }
}
