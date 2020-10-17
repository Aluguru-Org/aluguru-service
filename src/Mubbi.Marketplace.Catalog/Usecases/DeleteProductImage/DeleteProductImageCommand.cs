﻿using FluentValidation;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Catalog.Usecases.DeleteProductImage
{
    public class DeleteProductImageCommand : Command<DeleteProductImageCommandResponse>
    {
        public DeleteProductImageCommand(Guid productId, List<string> imageUrls)
        {
            ProductId = productId;
            ImageUrls = imageUrls;
        }

        public Guid ProductId { get; private set; }
        public List<string> ImageUrls { get; private set; }
    }

    public class DeleteProductImageCommandValidator : AbstractValidator<DeleteProductImageCommand>
    {
        public DeleteProductImageCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEqual(Guid.Empty);
            RuleFor(x => x.ImageUrls).NotEmpty();
        }
    }

    public class DeleteProductImageCommandResponse
    {
        public ProductViewModel Product { get; set; }
    }
}
