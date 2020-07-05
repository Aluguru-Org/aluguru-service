using FluentValidation;
using Mubbi.Marketplace.Catalog.Application.ViewModels;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Catalog.Application.Usecases.CreateProduct
{
    public class CreateProductCommand : Command<CreateProductCommandResponse>
    {
        public CreateProductCommand(Guid categoryId, Guid? subCategoryId, string name, string description, decimal price, bool isActive, int stockQuantity, 
            ERentType rentType, TimeSpan minRentTime, TimeSpan? maxRentTime, List<string> imageUrls, List<CustomField> customFields)
        {
            CategoryId = categoryId;
            SubCategoryId = subCategoryId;
            Name = name;
            Description = description;
            Price = price;
            IsActive = isActive;
            StockQuantity = stockQuantity;
            RentType = rentType;
            MinRentTime = minRentTime;
            MaxRentTime = maxRentTime;
            ImageUrls = imageUrls;
            CustomFields = customFields;
        }

        public Guid CategoryId { get; private set; }
        public Guid? SubCategoryId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; }
        public int StockQuantity { get; private set; }
        public ERentType RentType { get; private set; }
        public TimeSpan MinRentTime { get; private set; }
        public TimeSpan? MaxRentTime { get; private set; }
        public List<string> ImageUrls { get; private set; }
        public List<CustomField> CustomFields { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new CreateProductCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.ImageUrls).NotEmpty();

            When(x => x.MaxRentTime.HasValue, () =>
            {
                RuleFor(x => x.MinRentTime).LessThan(x => x.MaxRentTime);
            });
        }
    }

    public class CreateProductCommandResponse
    {
        public ProductViewModel Product { get; set; }
    }
}
