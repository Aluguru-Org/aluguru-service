﻿using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Register.ViewModels;
using System;

namespace Mubbi.Marketplace.Register.Usecases.UpadeUser
{
    public class UpdateUserCommand : Command<UpdateUserCommandResponse>
    {
        public UpdateUserCommand(Guid userId, string fullName, DocumentViewModel document, AddressViewModel address)
        {
            UserId = userId;
            FullName = fullName;
            Document = document;
            Address = address;
        }
        public Guid UserId { get; }
        public string FullName { get; }
        public DocumentViewModel Document { get; }
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
