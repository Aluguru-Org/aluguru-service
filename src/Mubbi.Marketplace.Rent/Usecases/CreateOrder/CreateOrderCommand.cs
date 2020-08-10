using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Rent.ViewModels;
using System;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Rent.Usecases.CreateOrder
{
    public class CreateOrderCommand : Command<CreateOrderCommandResponse>
    {
        public CreateOrderCommand(Guid userId, List<CreateOrderItemViewModel> orderItems)
        {
            UserId = userId;
            OrderItems = orderItems;
        }

        public Guid UserId { get; private set; }
        public List<CreateOrderItemViewModel> OrderItems { get; private set; }
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
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.OrderItems).NotEmpty().Must(x => 
            {
                return x.TrueForAll(item => item.ProductId != Guid.Empty);
            });
        }
    }

    public class CreateOrderCommandResponse
    {
        public OrderViewModel Order { get; set; }
    }
}

// Front
// User -> Adicionou item no carrinho -> Salva no LocalStorage id's produto [].

// Comprar -> SE Não estiver logado:
// Logar -> Criar pedido oficializando

// Back
// Cria pedido com id do usuário e lista de produtos