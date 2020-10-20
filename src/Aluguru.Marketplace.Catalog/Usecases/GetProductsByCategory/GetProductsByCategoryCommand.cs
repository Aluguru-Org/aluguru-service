using FluentValidation;
using Aluguru.Marketplace.Catalog.ViewModels;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Catalog.Usecases.GetProductsByCategory
{
    public class GetProductsByCategoryCommand : Command<GetProductsByCategoryCommandResponse>
    {
        public GetProductsByCategoryCommand(string category, PaginateCriteria paginateCriteria)
        {
            Category = category;
            PaginateCriteria = paginateCriteria;
        }

        public string Category { get; private set; }
        public PaginateCriteria PaginateCriteria { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new GetProductByCategoryCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class GetProductByCategoryCommandValidator : AbstractValidator<GetProductsByCategoryCommand>
    {
        public GetProductByCategoryCommandValidator()
        {
            RuleFor(x => x.Category).NotEmpty();
        }
    }

    public class GetProductsByCategoryCommandResponse
    {
        public PaginatedItem<Product> PaginatedProducts { get; set; }
    }
}
