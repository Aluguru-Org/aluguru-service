using Microsoft.AspNetCore.Identity;
using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Mubbi.Marketplace.Register.Domain.Models
{
    public class User : AggregateRoot
    {
        private User() { }
        public User(Email email, string password, ERoles role, string fullName, Address address, Document document)
            : base(NewId())
        {
            Password = password;
            Role = role;
            FullName = fullName;
            Email = email;
            Address = address;
            Document = document;
        }

        public Email Email { get; private set; }
        public string Password { get; private set; }
        public ERoles Role { get; private set; }        
        public string FullName { get; private set; }
        public Address Address { get; private set; }
        public Document Document { get; private set; }

        protected override void ValidateCreation()
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Password), "The field Password from User cannot be created null or empty");
            Ensure.NotEqual(ERoles.Admin, Role, "The field Role from User cannot be created as Admin");
        }
    }
}
