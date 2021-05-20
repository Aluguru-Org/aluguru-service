using Aluguru.Marketplace.Catalog.Usecases.UpdateCategory;
using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using static PampaDevs.Utils.Helpers.IdHelper;
using static PampaDevs.Utils.Helpers.DateTimeHelper;
using Aluguru.Marketplace.Catalog.Events;
using System.Text.RegularExpressions;

namespace Aluguru.Marketplace.Catalog.Domain
{
    public class Category : AggregateRoot
    {
        private readonly List<Category> _childrenCategories;
        private Category()
        {
            _childrenCategories = new List<Category>();
        }

        public Category(string name, string uri, bool highlights, Guid? mainCategoryId)
            : base(NewId())
        {
            MainCategoryId = mainCategoryId;
            Name = name;
            Uri = uri;
            Highlights = highlights;

            _childrenCategories = new List<Category>();

            ValidateEntity();
        }

        public string Name { get; private set; }
        public string Uri { get; private set; }
        public string ImageUrl { get; private set; }
        public bool Highlights { get; private set; }
        public Guid? MainCategoryId { get; private set; }
        public Category MainCategory { get; set; }
        public IReadOnlyCollection<Category> SubCategories { get { return _childrenCategories; } }
        
        // EF Relational
        public List<Product> Products { get; set; }

        public void UpdateImage(string url)
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(url), "The image url cannot be empty");
            ImageUrl = url;
        }

        public void RemoveImage()
        {
            ImageUrl = "";
        }

        public Category UpdateCategory(UpdateCategoryCommand command)
        {
            MainCategoryId = command.Category.MainCategoryId;
            Name = command.Category.Name;
            Uri = command.Category.Uri;
            Highlights = command.Category.Highlights;

            DateUpdated = NewDateTime();

            AddEvent(new CategoryUpdatedEvent(Id, Name, MainCategoryId));

            return this;
        }

        protected override void ValidateEntity()
        {            
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Name), "The field Name cannot be empty");
            Ensure.That<DomainException>(new Regex(@"^([\w-]+)$").IsMatch(Uri), "The field Uri should be in snake case.");
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
