using FluentValidation;
using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Catalog.Usecases.UpdateProduct
{
    public class UpdateProductCommand : Command<UpdateProductCommandResponse>
    {
        public UpdateProductCommand(Guid productId, UpdateProductDTO product)
        {
            ProductId = productId;
            Product = product;
        }

        public Guid ProductId { get; private set; }
        public UpdateProductDTO Product { get; private set; }        
    }

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Product.Name).NotEmpty();
            RuleFor(x => x.Product.Uri).Matches(@"^([\w-]+)$").WithMessage("The uri should be in snake case. Like 'video-game', 'mobile-app', 'cars'");
            RuleFor(x => x.Product.Description).NotEmpty();
            RuleFor(x => x.Product.ImageUrls).NotEmpty();

            When(x => x.Product.MaxRentDays.HasValue, () =>
            {
                RuleFor(x => x.Product.MinRentDays).LessThan(x => x.Product.MaxRentDays);
            });
        }
    }

    public class UpdateProductCommandResponse 
    {
        public ProductDTO Product { get; set; }
    }
}
