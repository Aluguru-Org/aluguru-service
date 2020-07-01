using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mubbi.Marketplace.Catalog.Domain
{
    [Table("Product")]
    public class Product : AggregateRoot
    {
        public Product()
        {

        }
        public Product(Guid categoryId, string name, string description, string imageUrl, decimal price, bool isActive, int stockQuantity, ERentType rentType, TimeSpan minRentTime, TimeSpan maxRentTime)
        {
            CategoryId = categoryId;
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            IsActive = isActive;
            StockQuantity = stockQuantity;
            RentType = rentType;
            MinRentTime = minRentTime;
            MaxRentTime = maxRentTime;

            ValidateCreation();
        }

        public Guid CategoryId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImageUrl { get; private set; }
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; }
        public int StockQuantity { get; private set; }
        public ERentType RentType { get; private set; }
        public TimeSpan MinRentTime { get; private set; }
        public TimeSpan? MaxRentTime { get; private set; }     

        //EF Relational
        public virtual Category Category { get; set; }        

        public void Active() => IsActive = true;
        public void Deactivate() => IsActive = false;

        public void UpdateRentType(ERentType rentType)
        {
            Ensure.Argument.IsNot(RentType == rentType, $"The Rent Type is already {RentType}");
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

        protected override void ValidateCreation()
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Name), "The field Name from product cannot be empty");
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Description), "The field Description from Product cannot be empty");
            Ensure.That<DomainException>(!string.IsNullOrEmpty(ImageUrl), "The field Image from Product cannot be empty");
            Ensure.That<DomainException>(CategoryId != Guid.Empty, "The field CategoryId from Product cannot be empty");
            Ensure.That<DomainException>(Price > 0, "The field Price from Product cannot be smaller or equal than zero");
            Ensure.That<DomainException>(StockQuantity > 0, "The field StockQuantity from Product cannot be smaller than zero");
            Ensure.That<DomainException>(MinRentTime <= MaxRentTime, "The field MinRentTime from Product cannot be greater than MaxRentTime");
        }
    }
}
