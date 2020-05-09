﻿using Mubbi.Marketplace.Shared.DomainObjects;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mubbi.Marketplace.Catalog.Domain
{
    [Table("Product")]
    public class Product : Entity, IAggregateRoot
    {
        public Product(Guid categoryId, string name, string description, string image, decimal price, bool isActive, int stockQuantity, ERentType rentType, TimeSpan minRentTime, TimeSpan maxRentTime, Dimensions dimensions)
        {
            CategoryId = categoryId;
            Name = name;
            Description = description;
            Image = image;
            Price = price;
            IsActive = isActive;
            StockQuantity = stockQuantity;
            RentType = rentType;
            Dimensions = dimensions;
            MinRentTime = minRentTime;
            MaxRentTime = maxRentTime;

            CreationDate = DateTime.UtcNow;

            ValidateCreation();
        }

        public Guid CategoryId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Image { get; private set; }
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; }
        public int StockQuantity { get; private set; }
        public ERentType RentType { get; private set; }
        public TimeSpan MinRentTime { get; private set; }
        public TimeSpan MaxRentTime { get; private set; }
        public DateTime CreationDate { get; private set; }
        public Dimensions Dimensions { get; private set; }

        //EF Relational
        public virtual Category Category { get; set; }

        public void Active() => IsActive = true;
        public void Deactivate() => IsActive = false;

        public void UpdateRentType(ERentType rentType)
        {
            EntityConcerns.IsEqual(RentType, rentType, $"The Rent Type is already {RentType}");
        }

        public void UpdateCategory(Category category)
        {
            EntityConcerns.IsNull(category, "The field category cannot be null");

            CategoryId = category.Id;
            Category = category;
        }

        public void UpdateDescription(string description)
        {
            EntityConcerns.IsEmpty(description, "The field Description from product cannot be empty");

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
            EntityConcerns.SmallerOrEqualThan(0, amount, "The amount cannot be smaller or equal than 0");

            StockQuantity += amount;
        }

        public bool HasStockFor(int amount)
        {
            return StockQuantity >= amount;
        }

        public override void ValidateCreation()
        {
            EntityConcerns.IsEmpty(Name, "The field Name from product cannot be empty");
            EntityConcerns.IsEmpty(Description, "The field Description from Product cannot be empty");
            EntityConcerns.IsEmpty(Image, "The field Image from Product cannot be empty");
            EntityConcerns.IsEqual(CategoryId, Guid.Empty, "The field CategoryId from Product cannot be empty");
            EntityConcerns.SmallerOrEqualThan(0, Price, "The field Price from Product cannot be smaller or equal than zero");
            EntityConcerns.SmallerThan(0, StockQuantity, "The field StockQuantity from Product cannot be smaller than zero");
            EntityConcerns.GreaterThan(MaxRentTime, MinRentTime, "The field MinRentTime from Product cannot be greater than MaxRentTime");
            EntityConcerns.IsNull(Dimensions, "The field Dimensions from Product cannot be null");
        }
    }
}
