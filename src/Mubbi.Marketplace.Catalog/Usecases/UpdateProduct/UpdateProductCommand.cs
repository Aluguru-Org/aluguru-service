﻿using FluentValidation;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Mubbi.Marketplace.Catalog.Usecases.UpdateProduct
{
    public class UpdateProductCommand : Command<UpdateProductCommandResponse>
    {
        public UpdateProductCommand(Guid productId, UpdateProductViewModel product)
        {
            ProductId = productId;
            Product = product;
        }

        public Guid ProductId { get; private set; }
        public UpdateProductViewModel Product { get; private set; }        
    }

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Product.Name).NotEmpty();
            RuleFor(x => x.Product.Description).NotEmpty();
            RuleFor(x => x.Product.ImageUrls).NotEmpty();

            When(x => x.Product.MaxRentDays.HasValue, () =>
            {
                RuleFor(x => x.Product.MinRentDays).LessThan(x => x.Product.MaxRentDays);
            });
        }
    }

    public class UpdateProductCommandResponse 
    {
        public ProductViewModel Product { get; set; }
    }
}
