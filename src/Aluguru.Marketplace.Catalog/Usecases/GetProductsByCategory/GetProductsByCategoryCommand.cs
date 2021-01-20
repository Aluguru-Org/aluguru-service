using FluentValidation;
using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Catalog.Usecases.GetProductsByCategory
{
    public class GetProductsByCategoryCommand : Command<GetProductsByCategoryCommandResponse>
    {
        public GetProductsByCategoryCommand(List<string> categories, PaginateCriteria paginateCriteria)
        {
            Categories = categories;
            PaginateCriteria = paginateCriteria;
        }

        public List<string> Categories { get; private set; }
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
            RuleFor(x => x.Categories).NotEmpty();
        }
    }

    public class GetProductsByCategoryCommandResponse
    {
        public PaginatedItem<ProductDTO> PaginatedProducts { get; set; }
    }
}
