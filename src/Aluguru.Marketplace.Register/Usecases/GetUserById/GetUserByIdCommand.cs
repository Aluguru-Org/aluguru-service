using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Register.Dtos;
using System;

namespace Aluguru.Marketplace.Register.Usecases.GetUserById
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
        public UserDTO User { get; set; }
    }
}
