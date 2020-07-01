using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.Application.Usecases.DeleteUser
{
    public class DeleteUserCommand : Command<bool>
    {
        public DeleteUserCommand(Guid id)
        {
            AggregateId = id;
            Id = id;
        }

        public Guid Id { get; private set; }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }       

    }

    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
        }
    }
}
