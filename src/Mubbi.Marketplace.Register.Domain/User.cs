using Microsoft.AspNetCore.Identity;
using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;

namespace Mubbi.Marketplace.Register.Domain.Models
{
    public class User : AggregateRoot
    {
        private User() { }
        public User(string username, int password, ERoles role, Name name, Email email, Address address, Document document)
        {
            Username = username;
            Password = password;
            Role = role;
            Name = name;
            Email = email;
            Address = address;
            Document = document;
        }

        public string Username { get; private set; }
        public int Password { get; private set; }
        public ERoles Role { get; private set; }        
        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public Address Address { get; private set; }
        public Document Document { get; private set; }

        protected override void ValidateCreation()
        {
            Ensure.NotNullOrEmpty(Username, "The field Username from User cannot be empty");
            Ensure.NotEqual(ERoles.Admin, Role, "The field Role from User cannot be created as Admin");
        }
    }
}
