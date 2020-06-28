using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Register.Domain
{
    public class Name : ValueObject
    {
        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

            ValidateCreation();
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }

        protected override void ValidateCreation()
        {
            Ensure.NotNullOrEmpty(FirstName, "The field FirstName cannot be empty");
            Ensure.NotNullOrEmpty(LastName, "The field LastName cannot be empty");
        }
    }
}
