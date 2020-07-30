﻿using FluentValidation;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Mubbi.Marketplace.Catalog.Usecases.UpdateCategory
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
        }
    }

    public class UpdateCategoryCommandResponse
    {
        public CategoryViewModel Category { get; set; }
    }
}
