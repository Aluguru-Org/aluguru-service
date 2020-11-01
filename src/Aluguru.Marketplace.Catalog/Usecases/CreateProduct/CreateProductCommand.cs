using FluentValidation;
using Aluguru.Marketplace.Catalog.ViewModels;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Catalog.Usecases.CreateProduct
{
    public class CreateProductCommand : Command<CreateProductCommandResponse>
    {
        public CreateProductCommand(Guid userId, Guid categoryId, Guid? subCategoryId, string name, string uri, string description, ERentType rentType, Price price, bool isActive, int stockQuantity, 
            int minRentDays, int? maxRentDays, int? minNoticeRentDays, List<CustomField> customFields)
        {
            UserId = userId;
            CategoryId = categoryId;
            SubCategoryId = subCategoryId;
            Name = name;
            Uri = uri;
            Description = description;
            RentType = rentType;
            Price = price;
            IsActive = isActive;
            StockQuantity = stockQuantity;
            MinRentDays = minRentDays;
            MaxRentDays = maxRentDays;
            MinNoticeRentDays = minNoticeRentDays;
            CustomFields = customFields;
        }

        public Guid UserId { get; private set; }
        public Guid CategoryId { get; private set; }
        public Guid? SubCategoryId { get; private set; }
        public string Name { get; private set; }
        public string Uri { get; private set; }
        public string Description { get; private set; }
        public ERentType RentType { get; private set; }
        public Price Price { get; private set; }
        public bool IsActive { get; private set; }
        public int StockQuantity { get; private set; }
        public int MinRentDays { get; private set; }
        public int? MaxRentDays { get; private set; }
        public int? MinNoticeRentDays { get; private set; }
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
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.CategoryId).NotEqual(Guid.Empty);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Uri).Matches(@"^([\w-]+)$").WithMessage("The uri should be in snake case. Like 'video-game', 'mobile-app', 'cars'");
            RuleFor(x => x.Description).NotEmpty();

            When(x => x.MaxRentDays.HasValue, () =>
            {
                RuleFor(x => x.MinRentDays).LessThan(x => x.MaxRentDays);
            });

            When(x => x.MinNoticeRentDays.HasValue, () =>
            {
                RuleFor(x => x.MinNoticeRentDays.Value).GreaterThan(0);
            });
        }
    }

    public class CreateProductCommandResponse
    {
        public ProductViewModel Product { get; set; }
    }
}
