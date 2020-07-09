using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Mubbi.Marketplace.Catalog.Domain
{
    public class Category : AggregateRoot
    {
        private readonly List<Category> _childrenCategories;
        private Category()
        {
            _childrenCategories = new List<Category>();
        }
        public Category(string name, int code, Guid? mainCategoryId)
            : base(NewId())
        {
            MainCategoryId = mainCategoryId;
            Name = name;
            Code = code;

            _childrenCategories = new List<Category>();

            ValidateCreation();
        }

        public string Name { get; private set; }
        public int Code { get; private set; }
        public Guid? MainCategoryId { get; private set; }
        public Category MainCategory { get; set; }
        public IReadOnlyCollection<Category> SubCategories { get { return _childrenCategories; } }
        public List<Product> Products { get; set; }
               

        protected override void ValidateCreation()
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Name), "The field Name cannot be empty");
            Ensure.That<DomainException>(Code > 0, "The field Code cannot be smaller or equal to 0");
            if (MainCategoryId != null)
            {
                Ensure.That<DomainException>(MainCategoryId != Guid.Empty, "The field MainCategoryId cannot be empty");
            }
        }

        public override string ToString()
        {
            return $"{Name} - {Code}";
        }
    }
}
