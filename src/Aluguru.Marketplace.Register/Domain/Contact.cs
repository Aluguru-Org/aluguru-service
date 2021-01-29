using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Text.RegularExpressions;
using static PampaDevs.Utils.Helpers.IdHelper;
using static PampaDevs.Utils.Helpers.DateTimeHelper;

namespace Aluguru.Marketplace.Register.Domain
{
    public class Contact : Entity
    {
        public Contact(string name, string phoneNumber, string email)
            : base(NewId())
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
        }
        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }        
        public User User { get; set; }

        internal void AssignUser(Guid userId)
        {
            UserId = userId;
        }

        public void UpdateContact(string name, string phoneNumber, string email)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;

            DateUpdated = NewDateTime();
        }

        protected override void ValidateEntity()
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Name), "The field Name from Contact cannot be created null or empty");
            Ensure.That<DomainException>(new Regex(@"^[a-zA-Z0-9._]+@[a-zA-Z0-9]+\.[a-z]+(?:\.[a-zA-Z]+)?").IsMatch(Email), "The field Email is not a valid E-mail Address");
        }
    }
}
