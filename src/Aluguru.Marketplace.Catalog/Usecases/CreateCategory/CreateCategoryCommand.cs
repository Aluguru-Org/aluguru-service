using FluentValidation;
using Aluguru.Marketplace.Catalog.ViewModels;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Catalog.Usecases.CreateCategory
{
    public class CreateCategoryCommand : Command<CreateCategoryCommandResponse>
    {
        public CreateCategoryCommand(string name, Guid? mainCategoryId = null)
        {
            Name = name;
            MainCategoryId = mainCategoryId;
        }

        public Guid? MainCategoryId { get; private set; }
        public string Name { get; private set; }

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
        }
    }

    public class CreateCategoryCommandResponse
    {
        public CategoryViewModel Category { get; set; }
    }
}
