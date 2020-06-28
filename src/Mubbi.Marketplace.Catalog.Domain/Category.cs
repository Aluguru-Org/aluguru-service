using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using System.Collections.Generic;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Mubbi.Marketplace.Catalog.Domain
{
    public class Category : AggregateRoot
    {
        public Category(string name, int code)
            : base(NewId())
        {
            Name = name;
            Code = code;

            ValidateCreation();
        }

        public string Name { get; private set; }
        public int Code { get; private set; }

        public ICollection<Product> Products { get; set; }

        protected override void ValidateCreation()
        {
            Ensure.NotNullOrEmpty(Name, "The field Name cannot be empty");
            Ensure.That(Code > 0, "The field Code cannot be smaller or equal to 0");            
        }

        public override string ToString()
        {
            return $"{Name} - {Code}";
        }
    }
}
