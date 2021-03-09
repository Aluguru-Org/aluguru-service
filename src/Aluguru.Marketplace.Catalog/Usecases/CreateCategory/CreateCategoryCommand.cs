using FluentValidation;
using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Catalog.Usecases.CreateCategory
{
    public class CreateCategoryCommand : Command<CreateCategoryCommandResponse>
    {
        public CreateCategoryCommand(string name, string uri, bool highlights, Guid? mainCategoryId = null)
        {
            Name = name;
            Uri = uri;
            Highlights = highlights;
            MainCategoryId = mainCategoryId;
        }

        public Guid? MainCategoryId { get; private set; }
        public string Name { get; private set; }
        public string Uri { get; private set; }
        public bool Highlights { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new CreateCategoryCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Uri).Matches(@"^([\w-]+)$").WithMessage("The category should be in snake case. Like 'video-game', 'mobile-app', 'cars'");
        }
    }

    public class CreateCategoryCommandResponse
    {
        public CategoryDTO Category { get; set; }
    }
}
