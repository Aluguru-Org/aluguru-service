using FluentValidation;
using Aluguru.Marketplace.Catalog.ViewModels;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Catalog.Usecases.UpdateCategory
{
    public class UpdateCategoryCommand : Command<UpdateCategoryCommandResponse>
    {
        public UpdateCategoryCommand(Guid categoryId, UpdateCategoryViewModel category)
        {
            CategoryId = categoryId;
            Category = category;
        }
        public Guid CategoryId { get; private set; }
        public UpdateCategoryViewModel Category { get; private set; }

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
            RuleFor(x => x.Category.Id).NotEqual(Guid.Empty);
            RuleFor(x => x.Category.Name).NotEmpty();
            RuleFor(x => x.Category.Uri).Matches(@"^([\w-]+)$").WithMessage("The category should be in snake case. Like 'video-game', 'mobile-app', 'cars'");
        }
    }

    public class UpdateCategoryCommandResponse
    {
        public CategoryViewModel Category { get; set; }
    }
}
