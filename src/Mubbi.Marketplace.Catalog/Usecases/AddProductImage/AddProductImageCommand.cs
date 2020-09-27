using FluentValidation;
using Microsoft.AspNetCore.Http;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Catalog.Usecases.AddProductImage
{
    public class AddProductImageCommand : Command<AddProductImageCommandResponse>
    {
        public AddProductImageCommand(Guid productId, List<IFormFile> files)
        {
            ProductId = productId;
            Files = files;
        }

        public Guid ProductId { get; private set; }
        public List<IFormFile> Files { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new AddProductImageCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddProductImageCommandValidator : AbstractValidator<AddProductImageCommand>
    {
        public AddProductImageCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEqual(Guid.Empty);
            RuleFor(x => x.Files).NotEmpty();
        }
    }

    public class AddProductImageCommandResponse
    {
        public ProductViewModel Product { get; set; }
    }
}
