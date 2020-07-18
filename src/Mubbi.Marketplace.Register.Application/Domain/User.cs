using Microsoft.AspNetCore.Identity;
using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Mubbi.Marketplace.Register.Domain
{
    public class User : AggregateRoot
    {
        private readonly List<Address> _addresses;
        private User() 
        {
            _addresses = new List<Address>();
        }
        public User(string email, string password, string fullName, Guid role)
            : base(NewId())
        {
            Password = password;
            FullName = fullName;
            Email = email;
            UserRoleId = role;

            _addresses = new List<Address>();

        }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string FullName { get; private set; }
        public Guid UserRoleId { get; private set; }
        public Document Document { get; private set; }
        public IReadOnlyCollection<Address> Addresses { get { return _addresses; } }
        // EF Relational
        public UserRole UserRole { get; set; }        

        protected override void ValidateCreation()
        {
            Ensure.That<DomainException>(new Regex(@"^[a-zA-Z0-9._]+@[a-zA-Z0-9]+\.[a-z]+(?:\.[a-zA-Z]+)?").IsMatch(Email), "The field Address from E-mail is not a valid E-mail Address");
            Ensure.That<DomainException>(!string.IsNullOrEmpty(Password), "The field Password from User cannot be created null or empty");
        }
    }
}
