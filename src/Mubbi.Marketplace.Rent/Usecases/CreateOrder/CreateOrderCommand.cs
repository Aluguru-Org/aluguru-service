using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;

namespace Mubbi.Marketplace.Rent.Usecases.CreateOrder
{
    public class CreateOrderCommand : Command<CreateOrderCommandResponse>
    {
        public override bool IsValid()
        {
            ValidationResult = new CreateOrderCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
        }
    }

    public class CreateOrderCommandResponse
    {

    }
}