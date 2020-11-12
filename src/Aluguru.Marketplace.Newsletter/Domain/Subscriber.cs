using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Text.RegularExpressions;

namespace Aluguru.Marketplace.Newsletter.Domain
{
    public class Subscriber : AggregateRoot
    {
        public Subscriber(string email)
            : base(Guid.NewGuid())
        {
            Email = email;

            ValidateEntity();
        }

        public string Email { get; set; }

        protected override void ValidateEntity()
        {
            Ensure.That<DomainException>(new Regex(@"^[a-zA-Z0-9._]+@[a-zA-Z0-9]+\.[a-z]+(?:\.[a-zA-Z]+)?").IsMatch(Email), "The E-mail provided is not a valid E-mail Address");
        }
    }
}
