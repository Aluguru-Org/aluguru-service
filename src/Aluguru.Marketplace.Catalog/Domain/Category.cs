using Aluguru.Marketplace.Catalog.Usecases.UpdateCategory;
using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using static PampaDevs.Utils.Helpers.IdHelper;
using static PampaDevs.Utils.Helpers.DateTimeHelper;
using Aluguru.Marketplace.Catalog.Events;

namespace Aluguru.Marketplace.Catalog.Domain
{
    public class Category : AggregateRoot
    {
        private readonly List<Category> _childrenCategories;
        private Category()
        {
            _childrenCategories = new List<Category>();
        }

        public Category(string name, Guid? mainCategoryId)
            : base(NewId())
        {
            MainCategoryId = mainCategoryId;
            Name = name;

            _childrenCategories = new List<Category>();

            ValidateEntity();
        }

        public string Name { get; private set; }
        public Guid? MainCategoryId { get; private set; }
        public Category MainCategory { get; set; }
        public IReadOnlyCollection<Category> SubCategories { get { return _childrenCategories; } }
        
        // EF Relational
        public List<Product> Products { get; set; }

        public Category UpdateCategory(UpdateCategoryCommand command)
        {
            MainCategoryId = command.Category.MainCategoryId;
            Name = command.Category.Name;

            DateUpdated = NewDateTime();

            AddEvent(new CategoryUpdatedEvent(Id, Name, MainCategoryId));

            return this;
        }

        protected override void ValidateEntity()
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Name), "The field Name cannot be empty");
            if (MainCategoryId != null)
            {
                Ensure.That<DomainException>(MainCategoryId != Guid.Empty, "The field MainCategoryId cannot be empty");
            }
        }

        public override string ToString()
        {
            return $"{Name} - {Id}";
        }
    }
}
