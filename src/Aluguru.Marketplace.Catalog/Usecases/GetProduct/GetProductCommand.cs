using FluentValidation;
using Aluguru.Marketplace.Catalog.ViewModels;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;

namespace Aluguru.Marketplace.Catalog.Usecases.GetProduct
{
    public class GetProductCommand : Command<GetProductCommandResponse>
    {
        public GetProductCommand(string productUri)
        {
            ProductUri = productUri;
        }

        public string ProductUri { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new GetProductCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class GetProductCommandValidator : AbstractValidator<GetProductCommand>
    {
        public GetProductCommandValidator()
        {
            RuleFor(x => x.ProductUri).NotEmpty();
        }
    }

    public class GetProductCommandResponse
    {
        public ProductViewModel Product { get; set; }
    }
}
