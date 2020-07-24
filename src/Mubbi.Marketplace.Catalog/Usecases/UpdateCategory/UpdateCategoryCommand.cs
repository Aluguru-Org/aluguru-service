using FluentValidation;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Mubbi.Marketplace.Catalog.Usecases.UpdateCategory
{
    public class UpdateCategoryCommand : Command<UpdateCategoryCommandResponse>
    {
        public UpdateCategoryCommand(Guid categoryId, Guid? mainCategoryId, string name)
        {
            CategoryId = categoryId;
            MainCategoryId = mainCategoryId;
            Name = name;
        }

        public Guid CategoryId { get; private set; }
        public Guid? MainCategoryId { get; private set; }
        public string Name { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateCategoryCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryId).NotEqual(Guid.Empty);
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class UpdateCategoryCommandResponse
    {
        public CategoryViewModel Category { get; set; }
    }
}
