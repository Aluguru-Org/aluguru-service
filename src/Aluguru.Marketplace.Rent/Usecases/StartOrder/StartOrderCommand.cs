using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.ViewModels;
using FluentValidation;
using System;

namespace Aluguru.Marketplace.Rent.Usecases.StartOrder
{
    public class StartOrderCommand : Command<StartOrderCommandResponse>
    {
        public Guid OrderId { get; private set; }
        public string Token { get; private set; }
        

    }

    public class CreateOrderCommandValidator : AbstractValidator<StartOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEqual(Guid.Empty);
            RuleFor(x => x.Token).NotEmpty();
        }
    }

    public class StartOrderCommandResponse
    {
        public OrderViewModel Order { get; set; }
    }
}
