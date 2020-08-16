using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Rent.ViewModels;
using System;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Rent.Usecases.UpdateOrder
{
    public class UpdateOrderCommand : Command<UpdateOrderCommandResponse>
    {
        public UpdateOrderCommand(Guid orderId, List<CreateOrderItemViewModel> orderItems)
        {
            OrderId = orderId;
            OrderItems = orderItems;
        }

        public Guid OrderId { get; private set; }
        public List<CreateOrderItemViewModel> OrderItems { get; private set; }
        public override bool IsValid()
        {
            ValidationResult = new CreateOrderCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEqual(Guid.Empty);
            RuleFor(x => x.OrderItems).NotEmpty().Must(x => 
            {
                return x.TrueForAll(item => item.ProductId != Guid.Empty);
            });
        }
    }

    public class UpdateOrderCommandResponse
    {
        public OrderViewModel Order { get; set; }
    }
}

// Front
// User -> Adicionou item no carrinho -> Salva no LocalStorage id's produto [].

// Comprar -> SE N�o estiver logado:
// Logar -> Criar pedido oficializando

// Back
// Cria pedido com id do usu�rio e lista de produtos