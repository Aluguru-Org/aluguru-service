using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Mubbi.Marketplace.Catalog.Domain
{
    public class RentPeriod : AggregateRoot
    {
        public RentPeriod(string name, int days)
            : base(NewId())
        {
            Name = name;
            Days = days;

            ValidateEntity();
        }

        public string Name { get; set; }
        public int Days { get; set; }

        protected override void ValidateEntity()
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Name), "The field Name cannot be empty");
            Ensure.That<DomainException>(Days > 0, "The field Days cannot be less than one");
        }
    }
}
