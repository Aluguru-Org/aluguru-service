using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Catalog.Usecases.AddCategoryImage
{
    public class UpdateCategoryImageCommand : Command<UpdateCategoryImageCommandResponse>
    {
        public UpdateCategoryImageCommand(Guid categoryId, IFormFile file)
        {
            CategoryId = categoryId;
            File = file;
        }

        public Guid CategoryId { get; private set; }
        public IFormFile File { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateCategoryImageCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateCategoryImageCommandValidator : AbstractValidator<UpdateCategoryImageCommand>
    {
        public UpdateCategoryImageCommandValidator()
        {
            RuleFor(x => x.CategoryId).NotEqual(Guid.Empty);
            RuleFor(x => x.File).NotNull();
        }
    }

    public class UpdateCategoryImageCommandResponse
    {
        public CategoryDTO Category { get; set; }
    }
}
