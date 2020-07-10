using FluentValidation;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Mubbi.Marketplace.Catalog.Usecases.UpdateCategory
{
    public class UpdateCategoryCommand : Command<UpdateCategoryCommandResponse>
    {
        public UpdateCategoryCommand(Guid? mainCategoryId, string name, int code)
        {
            MainCategoryId = mainCategoryId;
            Name = name;
            Code = code;
        }

        public Guid? MainCategoryId { get; private set; }
        public string Name { get; private set; }
        public int Code { get; private set; }

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
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).GreaterThanOrEqualTo(0);
        }
    }

    public class UpdateCategoryCommandResponse
    {
        public CategoryViewModel Category { get; set; }
    }
}
