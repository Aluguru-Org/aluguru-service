using Mubbi.Marketplace.Domain.Core.Concerns;
using Mubbi.Marketplace.Domain.Core.Exceptions;
using Mubbi.Marketplace.Domain.Core.Models;
using Mubbi.Marketplace.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mubbi.Marketplace.Domain.Models
{
    [Table("Product")]
    public class Product : Entity
    {
        public Product(Guid categoryId, string name, string description, string image, decimal price, bool isActive, int stockQuantity, Dimensions dimensions)
        {
            CategoryId = categoryId;
            Name = name;
            Description = description;
            Image = image;
            Price = price;
            IsActive = isActive;
            StockQuantity = stockQuantity;
            Dimensions = dimensions;

            CreationDate = DateTime.UtcNow;

            Validate();
        }

        public Guid CategoryId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Image { get; private set; }
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; }
        public int StockQuantity { get; private set; }
        public DateTime CreationDate { get; private set; }
        public Dimensions Dimensions { get; private set; }
        public Category Category { get; private set; }

        public void Active() => IsActive = true;
        public void Deactivate() => IsActive = false;

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
            if (!HasStockFor(amount)) throw new DomainException("Insufficient stock");

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

        public override void Validate()
        {
            EntityConcerns.IsEmpty(Name, "The field Name from product cannot be empty");
            EntityConcerns.IsEmpty(Description, "The field Description from Product cannot be empty");
            EntityConcerns.IsEmpty(Image, "The field Image from Product cannot be empty");
            EntityConcerns.IsEqual(CategoryId, Guid.Empty, "The field CategoryId from Product cannot be empty");
            EntityConcerns.SmallerOrEqualThan(0, Price, "The field Price from Product cannot be smaller or equal than zero");
            EntityConcerns.SmallerThan(0, StockQuantity, "The field StockQuantity from Product cannot be smaller than zero");
        }
    }
}
