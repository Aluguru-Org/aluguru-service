using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Aluguru.Marketplace.Register.Domain
{
    public class Email : ValueObject
    {
        public Email(string address)
        {
            Address = address;

            ValidateValueObject();
        }

        public string Address { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
        }

        protected override void ValidateValueObject()
        {
            Ensure.That(new Regex(@"^[a-zA-Z0-9._]+@[a-zA-Z0-9]+\.[a-z]+(?:\.[a-zA-Z]+)?").IsMatch(Address), "The field Address from E-mail is not a valid E-mail Address");
        }
    }
}
