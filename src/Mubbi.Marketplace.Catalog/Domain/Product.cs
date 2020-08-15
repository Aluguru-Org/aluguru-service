using Mubbi.Marketplace.Catalog.Events;
using Mubbi.Marketplace.Catalog.Usecases.UpdateProduct;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using static PampaDevs.Utils.Helpers.DateTimeHelper;


namespace Mubbi.Marketplace.Catalog.Domain
{
    [Table("Product")]
    public class Product : AggregateRoot
    {
        private readonly List<string> _imageUrls;
        private readonly List<CustomField> _customFields;

        private Product()
        {
            _customFields = new List<CustomField>();
        }

        public Product(Guid userId, Guid categoryId, Guid? subCategoryId, string name, string description, ERentType rentType, Price price, bool isActive, int stockQuantity, int minRentDays, int? maxRentDays, int? minNoticeRentDays, List<string> imageUrls, List<CustomField> customFields)
        {
            UserId = userId;
            CategoryId = categoryId;
            SubCategoryId = subCategoryId;
            Name = name;
            Description = description;
            RentType = rentType;
            Price = price;
            IsActive = isActive;
            StockQuantity = stockQuantity;
            MinRentDays = minRentDays;
            MaxRentDays = maxRentDays;
            MinNoticeRentDays = minNoticeRentDays;

            _imageUrls = imageUrls;
            _customFields = customFields;

            ValidateEntity();
        }
        public Guid UserId { get; private set; }
        public Guid CategoryId { get; private set; }
        public Guid? SubCategoryId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ERentType RentType { get; private set; }
        public Price Price { get; set; }  
        public bool IsActive { get; private set; }
        public int StockQuantity { get; private set; }
        public int MinRentDays { get; private set; }
        public int? MaxRentDays { get; private set; }
        public int? MinNoticeRentDays { get; private set; }
        public IReadOnlyCollection<string> ImageUrls { get { return _imageUrls; } }
        public IReadOnlyCollection<CustomField> CustomFields { get { return _customFields; } }

        //EF Relational
        public virtual Category Category { get; set; }
        public virtual Category SubCategory { get; set; }

        public void Active() => IsActive = true;
        public void Deactivate() => IsActive = false;

        public Product UpdateProduct(UpdateProductCommand command)
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(command.Product.Name), "The field Name from product cannot be empty");
            Ensure.That<DomainException>(!string.IsNullOrEmpty(command.Product.Description), "The field Description from Product cannot be empty");
            Ensure.That<DomainException>(command.Product.CategoryId != Guid.Empty, "The field CategoryId from Product cannot be empty");            
            Ensure.That<DomainException>(command.Product.StockQuantity > 0, "The field StockQuantity from Product cannot be smaller than zero");
            
            if (command.Product.MaxRentDays.HasValue)
            {
                Ensure.That<DomainException>(command.Product.MinRentDays <= command.Product.MaxRentDays, "The field MinRentTime from Product cannot be greater than MaxRentTime");
            }

            if (command.Product.MinNoticeRentDays.HasValue)
            {
                Ensure.That<DomainException>(command.Product.MinNoticeRentDays.Value > 0, "The field MinNoticeRentDays from Product cannot be less than one");
            }

            Ensure.That<DomainException>(command.Product.ImageUrls != null && command.Product.ImageUrls.Count >= 1, "The field ImageUrls from Product cannot be empty");

            if (command.Product.SubCategoryId.HasValue)
            {
                Ensure.That<DomainException>(SubCategoryId != Guid.Empty, "The field SubCategoryId from Product cannot be empty");
            }

            CategoryId = command.Product.CategoryId;
            SubCategoryId = command.Product.SubCategoryId;
            Name = command.Product.Name;
            Description = command.Product.Description;            
            StockQuantity = command.Product.StockQuantity;
            MinRentDays = command.Product.MinRentDays;
            MaxRentDays = command.Product.MaxRentDays;
            IsActive = command.Product.IsActive;

            Price.UpdateSellPrice(command.Product.Price.SellPrice);
            Price.UpdateDailyRentPrice(command.Product.Price.DailyRentPrice);
            Price.UpdatePeriodRentPrices(command.Product.Price.PeriodRentPrices);

            _imageUrls.Clear();
            _imageUrls.AddRange(command.Product.ImageUrls);

            DateUpdated = NewDateTime();

            AddEvent(new ProductUpdatedEvent(Id, this));

            return this;
        }

        public void UpdateCategory(Category category)
        {
            Ensure.Argument.NotNull(category, "The field category cannot be null");

            CategoryId = category.Id;
            Category = category;
        }

        public void UpdateDescription(string description)
        {
            Ensure.Argument.NotNullOrEmpty(description, "The field Description from product cannot be empty");

            Description = description;
        }

        public void DebitStock(int amount)
        {
            if (amount < 0) amount *= -1;
            if (!HasStockFor(amount)) throw new DomainException($"Insufficient stock. Only the amount of {amount} is avaiable");

            StockQuantity -= amount;
        }

        public void ReplenishStock(int amount)
        {
            Ensure.Argument.Is(amount > 0, "The amount cannot be smaller or equal than 0");

            StockQuantity += amount;
        }

        public bool HasStockFor(int amount)
        {
            return StockQuantity >= amount;
        }

        protected override void ValidateEntity()
        {
            Ensure.That<DomainException>(UserId != Guid.Empty, "The field UserId from Product cannot be empty");
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Name), "The field Name from product cannot be empty");
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Description), "The field Description from Product cannot be empty");
            Ensure.That<DomainException>(CategoryId != Guid.Empty, "The field CategoryId from Product cannot be empty");
            Ensure.That<DomainException>(Price != null, "The field Price from Product cannot cannot be null");
            Ensure.That<DomainException>(StockQuantity > 0, "The field StockQuantity from Product cannot be smaller than zero");

            if (MaxRentDays.HasValue)
            {
                Ensure.That<DomainException>(MinRentDays <= MaxRentDays.Value, "The field MinRentTime from Product cannot be greater than MaxRentTime");
            }

            if (MinNoticeRentDays.HasValue)
            {
                Ensure.That<DomainException>(MinNoticeRentDays.Value > 0, "The field MinNoticeRentDays from Product cannot be less than one");
            }

            Ensure.That<DomainException>(ImageUrls != null && ImageUrls.Count >= 1, "The field ImageUrls from Product cannot be empty");

            if (SubCategoryId.HasValue)
            {
                Ensure.That<DomainException>(SubCategoryId != Guid.Empty, "The field SubCategoryId from Product cannot be empty");
            }
        }
    }
}