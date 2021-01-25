using Aluguru.Marketplace.Catalog.Events;
using Aluguru.Marketplace.Catalog.Usecases.UpdateProduct;
using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using static PampaDevs.Utils.Helpers.DateTimeHelper;
using static PampaDevs.Utils.Helpers.IdHelper;


namespace Aluguru.Marketplace.Catalog.Domain
{
    [Table("Product")]
    public class Product : AggregateRoot
    {
        private readonly List<DateTime> _blockedDates;
        private readonly List<string> _imageUrls;
        private readonly List<CustomField> _customFields;

        private Product()
        {
            _blockedDates = new List<DateTime>();
            _imageUrls = new List<string>();
            _customFields = new List<CustomField>();
        }

        public Product(Guid userId, Guid categoryId, Guid? subCategoryId, string name, string uri, string description, ERentType rentType, Price price, bool isActive, int stockQuantity, int minRentDays, int? maxRentDays, int? minNoticeRentDays, List<CustomField> customFields)
            : base(NewId())
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

            _blockedDates = new List<DateTime>();
            _imageUrls = new List<string>();
            _customFields = customFields;

            ValidateEntity();
        }
        public Guid UserId { get; private set; }
        public Guid CategoryId { get; private set; }
        public Guid? SubCategoryId { get; private set; }
        public string Name { get; private set; }
        public string Uri {get; private set; }
        public string Description { get; private set; }
        public ERentType RentType { get; private set; }
        public Price Price { get; set; }
        public bool IsActive { get; private set; }
        public int StockQuantity { get; private set; }
        public int MinRentDays { get; private set; }        
        public int? MaxRentDays { get; private set; }
        public int? MinNoticeRentDays { get; private set; }
        public IReadOnlyCollection<DateTime> BlockedDates { get { return _blockedDates; } }
        public IReadOnlyCollection<string> ImageUrls { get { return _imageUrls; } }
        public IReadOnlyCollection<CustomField> CustomFields { get { return _customFields; } }

        //EF Relational
        public virtual Category Category { get; set; }
        public virtual Category SubCategory { get; set; }

        public void Active() => IsActive = true;
        public void Deactivate() => IsActive = false;

        public void AddImage(string url)
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(url), "The image url cannot be empty");

            DateUpdated = NewDateTime();

            _imageUrls.Add(url);
        }

        public bool RemoveImage(string url)
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(url), "The image url to be removed cannot be empty");

            DateUpdated = NewDateTime();

            return _imageUrls.Remove(url);
        }

        public bool CheckValidRentStartDate(DateTime rentStartDate)
        {
            var minRentStartDate = NewDateTime();
            if (MinNoticeRentDays.HasValue)
            {
                minRentStartDate = minRentStartDate.AddDays(MinNoticeRentDays.Value);
            }
            return rentStartDate > minRentStartDate;
        }

        public bool CheckValidRentDays(int rentDays)
        {
            return MinRentDays < rentDays && (MaxRentDays.HasValue ? rentDays < MaxRentDays.Value : true);
        }

        public Product UpdateProduct(UpdateProductCommand command)
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(command.Product.Name), "The field Name from product cannot be empty");
            Ensure.That<DomainException>(new Regex(@"^([\w-]+)$").IsMatch(command.Product.Uri), "The field Uri should be in snake case.");
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

            if (command.Product.SubCategoryId.HasValue)
            {
                Ensure.That<DomainException>(SubCategoryId != Guid.Empty, "The field SubCategoryId from Product cannot be empty");
            }

            CategoryId = command.Product.CategoryId;
            SubCategoryId = command.Product.SubCategoryId;
            Name = command.Product.Name;
            Uri = command.Product.Uri;
            Description = command.Product.Description;            
            StockQuantity = command.Product.StockQuantity;
            MinRentDays = command.Product.MinRentDays;
            MaxRentDays = command.Product.MaxRentDays;
            IsActive = command.Product.IsActive;

            _blockedDates.Clear();
            _blockedDates.AddRange(command.Product.BlockedDates);

            _customFields.Clear();

            foreach (var customField in command.Product.CustomFields)
            {
                var fieldType = (EFieldType)Enum.Parse(typeof(EFieldType), customField.FieldType);
                CustomField newCustomField = null;

                switch(fieldType)
                {
                    case EFieldType.Text:
                    case EFieldType.Number:
                        newCustomField = new CustomField(fieldType, customField.FieldName);
                        break;
                    case EFieldType.Checkbox:
                    case EFieldType.Radio:
                        newCustomField = new CustomField(fieldType, customField.FieldName, customField.ValueAsOptions);
                        break;
                }
                _customFields.Add(newCustomField);
            }

            Price.UpdateFreightPriceByKM(command.Product.Price.FreightPriceKM);
            Price.UpdateSellPrice(command.Product.Price.SellPrice);
            Price.UpdateDailyRentPrice(command.Product.Price.DailyRentPrice);
            Price.UpdatePeriodRentPrices(command.Product.Price.PeriodRentPrices);

            DateUpdated = NewDateTime();

            AddEvent(new ProductUpdatedEvent(Id, this));

            return this;
        }

        public void UpdateCategory(Category category)
        {
            Ensure.Argument.NotNull(category, "The field category cannot be null");

            CategoryId = category.Id;
            Category = category;

            DateUpdated = NewDateTime();
        }

        public void UpdateDescription(string description)
        {
            Ensure.Argument.NotNullOrEmpty(description, "The field Description from product cannot be empty");

            Description = description;
            DateUpdated = NewDateTime();
        }

        public void DebitStock(int amount)
        {
            if (amount < 0) amount *= -1;
            if (!HasStockFor(amount)) throw new DomainException($"Insufficient stock. Only the amount of {amount} is avaiable");

            StockQuantity -= amount;
            DateUpdated = NewDateTime();
        }

        public void ReplenishStock(int amount)
        {
            Ensure.Argument.Is(amount > 0, "The amount cannot be smaller or equal than 0");

            StockQuantity += amount;
            DateUpdated = NewDateTime();
        }

        public bool HasDateAvaiabilityFor(DateTime rentStartDate)
        {
            return !_blockedDates.Any(date => date.Date == rentStartDate.Date);
        }

        public bool HasStockFor(int amount)
        {
            return StockQuantity >= amount;
        }

        public bool HasCategory(string categoryUri)
        {
            if (Category != null && Category.Uri.Trim().ToLower() == categoryUri) return true;
            if (SubCategory != null && SubCategory.Uri.Trim().ToLower() == categoryUri) return true;

            return false;
        }

        protected override void ValidateEntity()
        {
            Ensure.That<DomainException>(UserId != Guid.Empty, "The field UserId from Product cannot be empty");
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Name), "The field Name from product cannot be empty");
            Ensure.That<DomainException>(new Regex(@"^([\w-]+)$").IsMatch(Uri), "The field Uri should be in snake case.");
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

            if (SubCategoryId.HasValue)
            {
                Ensure.That<DomainException>(SubCategoryId != Guid.Empty, "The field SubCategoryId from Product cannot be empty");
            }
        }
    }
}