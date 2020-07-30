using FluentValidation;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Catalog.Usecases.DeleteProduct
{
    public class DeleteProductCommand : Command<bool>
    {
        public DeleteProductCommand(Guid categoryId)
        {
            ProductId = categoryId;
        }

        public Guid ProductId { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteProductCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEqual(Guid.Empty);
        }
    }
}
