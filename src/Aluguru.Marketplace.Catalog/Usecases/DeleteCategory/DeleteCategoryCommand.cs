using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Catalog.Usecases.DeleteCategory
{
    public class DeleteCategoryCommand : Command<bool>
    {
        public DeleteCategoryCommand(Guid categoryId)
        {
            CategoryId = categoryId;
        }

        public Guid CategoryId { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteCategoryCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryId).NotEqual(Guid.Empty);
        }
    }
}
