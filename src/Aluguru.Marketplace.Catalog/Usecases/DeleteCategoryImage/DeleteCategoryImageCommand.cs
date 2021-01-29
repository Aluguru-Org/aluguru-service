using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using FluentValidation;
using System;

namespace Aluguru.Marketplace.Catalog.Usecases.DeleteCategoryImage
{
    public class DeleteCategoryImageCommand : Command<DeleteCategoryImageCommandResponse>
    {
        public DeleteCategoryImageCommand(Guid categoryId)
        {
            CategoryId = categoryId;
        }

        public Guid CategoryId { get; private set; }
    }

    public class DeleteCategoryImageCommandValidator : AbstractValidator<DeleteCategoryImageCommand>
    {
        public DeleteCategoryImageCommandValidator()
        {
            RuleFor(x => x.CategoryId).NotEqual(Guid.Empty);
        }
    }

    public class DeleteCategoryImageCommandResponse
    {
        public CategoryDTO Category { get; set; }
    }
}
