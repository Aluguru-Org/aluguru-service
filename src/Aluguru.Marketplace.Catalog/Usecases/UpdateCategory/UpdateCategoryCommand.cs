using FluentValidation;
using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Catalog.Usecases.UpdateCategory
{
    public class UpdateCategoryCommand : Command<UpdateCategoryCommandResponse>
    {
        public UpdateCategoryCommand(Guid categoryId, UpdateCategoryDTO category)
        {
            CategoryId = categoryId;
            Category = category;
        }
        public Guid CategoryId { get; private set; }
        public UpdateCategoryDTO Category { get; private set; }

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
        public CategoryDTO Category { get; set; }
    }
}
