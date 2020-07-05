using FluentValidation;
using Mubbi.Marketplace.Catalog.Application.ViewModels;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Catalog.Application.Usecases.CreateCategory
{
    public class CreateCategoryCommand : Command<CreateCategoryCommandResponse>
    {
        public CreateCategoryCommand(string name, int code)
        {
            Name = name;
            Code = code;
        }

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
