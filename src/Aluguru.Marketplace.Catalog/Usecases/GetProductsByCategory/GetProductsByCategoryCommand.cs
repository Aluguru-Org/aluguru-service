using FluentValidation;
using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;

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
        public PaginatedItem<ProductDTO> PaginatedProducts { get; set; }
    }
}
