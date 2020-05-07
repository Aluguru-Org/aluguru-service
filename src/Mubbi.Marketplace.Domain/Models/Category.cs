﻿using Mubbi.Marketplace.Domain.Core.Concerns;
using Mubbi.Marketplace.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Domain.Models
{
    public class Category : Entity
    {
        public Category(string name, int code)
        {
            Name = name;
            Code = code;

            Validate();
        }

        public string Name { get; private set; }
        public int Code { get; private set; }

        public ICollection<Product> Products { get; private set; }

        public override void Validate()
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
