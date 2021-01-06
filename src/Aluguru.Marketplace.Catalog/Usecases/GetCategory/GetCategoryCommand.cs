using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using FluentValidation;

namespace Aluguru.Marketplace.Catalog.Usecases.GetCategory
{
    public class GetCategoryCommand : Command<GetCategoryCommandResponse>
    {
        public GetCategoryCommand(string categoryUri)
        {
            CategoryUri = categoryUri;
        }

        public string CategoryUri { get; private set; }
        public override bool IsValid()
        {
            ValidationResult = new GetCategoryCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class GetCategoryCommandValidator : AbstractValidator<GetCategoryCommand>
    {
        public GetCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryUri).NotEmpty();
        }
    }

    public class GetCategoryCommandResponse
    {
        public CategoryDTO Category { get; set; }
    }
}
