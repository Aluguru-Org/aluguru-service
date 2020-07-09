﻿using FluentValidation;
using Mubbi.Marketplace.Catalog.Application.ViewModels;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Catalog.Application.Usecases.GetProduct
{
    public class GetProductCommand : Command<GetProductCommandResponse>
    {
        public GetProductCommand(Guid productId)
        {
            ProductId = productId;
        }

        public Guid ProductId { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new GetProductCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class GetProductCommandValidator : AbstractValidator<GetProductCommand>
    {
        public GetProductCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEqual(Guid.Empty);
        }
    }

    public class GetProductCommandResponse
    {
        public ProductViewModel Product { get; set; }
    }
}
