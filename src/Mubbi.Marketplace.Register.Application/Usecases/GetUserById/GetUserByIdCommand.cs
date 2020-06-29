using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Register.Application.ViewModels;
using System;

namespace Mubbi.Marketplace.Register.Application.Usecases.GetUserById
{
    public class GetUserByIdCommand : Command<GetUserByIdCommandResponse>
    {
        public GetUserByIdCommand(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }
        public override bool IsValid()
        {
            ValidationResult = new GetUserByIdCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class GetUserByIdCommandValidator : AbstractValidator<GetUserByIdCommand>
    {
        public GetUserByIdCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
        }
    }

    public class GetUserByIdCommandResponse
    {
        public UserViewModel User { get; set; }
    }
}
