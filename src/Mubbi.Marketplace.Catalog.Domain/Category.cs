﻿using Mubbi.Marketplace.Shared.DomainObjects;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Catalog.Domain
{
    public class Category : Entity
    {
        public Category(string name, int code)
        {
            Name = name;
            Code = code;

            ValidateCreation();
        }

        public string Name { get; private set; }
        public int Code { get; private set; }

        public ICollection<Product> Products { get; set; }

        public override void ValidateCreation()
        {
            EntityConcerns.IsEmpty(Name, "The field Name cannot be empty");
            EntityConcerns.SmallerOrEqualThan(0, Code, "The field Code smaller or equal than 0");
        }

        public override string ToString()
        {
            return $"{Name} - {Code}";
        }
    }
}
