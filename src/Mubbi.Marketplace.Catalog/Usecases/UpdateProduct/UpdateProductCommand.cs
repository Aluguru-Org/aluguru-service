using FluentValidation;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Catalog.Usecases.UpdateProduct
{
    public class UpdateProductCommand : Command<UpdateProductCommandResponse>
    {
        public UpdateProductCommand(Guid productId, Guid categoryId, Guid? subCategoryId, string name, string description, decimal price, bool isActive, int stockQuantity, int minRentDays, int? maxRentDays, List<string> imageUrls, List<CustomField> customFields)
        {
            ProductId = productId;
            CategoryId = categoryId;
            SubCategoryId = subCategoryId;
            Name = name;
            Description = description;
            Price = price;
            IsActive = isActive;
            StockQuantity = stockQuantity;
            MinRentDays = minRentDays;
            MaxRentDays = maxRentDays;
            ImageUrls = imageUrls;
            CustomFields = customFields;
        }

        public Guid ProductId { get; private set; }
        public Guid CategoryId { get; private set; }
        public Guid? SubCategoryId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; }
        public int StockQuantity { get; private set; }
        public int MinRentDays { get; private set; }
        public int? MaxRentDays { get; private set; }
        public List<string> ImageUrls { get; private set; }
        public List<CustomField> CustomFields { get; private set; }
    }

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.ImageUrls).NotEmpty();

            When(x => x.MaxRentDays.HasValue, () =>
            {
                RuleFor(x => x.MinRentDays).LessThan(x => x.MaxRentDays);
            });
        }
    }

    public class UpdateProductCommandResponse 
    {
        public ProductViewModel Product { get; set; }
    }
}
