using FluentValidation;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Mubbi.Marketplace.Catalog.Usecases.CreateCategory
{
    public class CreateCategoryCommand : Command<CreateCategoryCommandResponse>
    {
        public CreateCategoryCommand(string name, int code, Guid? mainCategoryId = null)
        {
            Name = name;
            Code = code;
            MainCategoryId = mainCategoryId;
        }

        public Guid? MainCategoryId { get; private set; }
        public string Name { get; private set; }
        public int Code { get; private set; }

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
            RuleFor(x => x.Code).GreaterThanOrEqualTo(0);
        }
    }

    public class CreateCategoryCommandResponse
    {
        public CategoryViewModel Category { get; set; }
    }
}
