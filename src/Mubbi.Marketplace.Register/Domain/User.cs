using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure;
using Mubbi.Marketplace.Register.Usecases.UpadeUser;
using PampaDevs.Utils;
using System;
using System.Text.RegularExpressions;
using Mubbi.Marketplace.Register.Events;
using static PampaDevs.Utils.Helpers.IdHelper;
using static PampaDevs.Utils.Helpers.DateTimeHelper;

namespace Mubbi.Marketplace.Register.Domain
{
    public class User : AggregateRoot
    {
        private User() { }

        public User(Guid id, string email, string password, string fullName, Guid role) 
            : base(id)
        {
            Password = password;
            FullName = fullName;
            Email = email;
            UserRoleId = role;

            ValidateEntity();
        }

        public User(string email, string password, string fullName, Guid role)
            : base(NewId())
        {
            Password = password;
            FullName = fullName;
            Email = email;
            UserRoleId = role;

            ValidateEntity();
        }

        
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string FullName { get; private set; }
        public Guid UserRoleId { get; private set; }
        public Document Document { get; private set; }
        public Address Address { get; private set; }
        // EF Relational
        public UserRole UserRole { get; set; }

        public User UpdateUser(UpdateUserCommand command)
        {
            if (FullName != command.FullName)
            {
                FullName = command.FullName;
            }

            if (Document == null || Document.Number != command.Document.Number)
            {
                Document = command.Document;
            }

            Address = command.Address;
            
            ValidateEntity();

            DateUpdated = NewDateTime();

            AddEvent(new UserUpdatedEvent(Id, this));

            return this;
        }

        protected override void ValidateEntity()
        {
            Ensure.That<DomainException>(new Regex(@"^[a-zA-Z0-9._]+@[a-zA-Z0-9]+\.[a-z]+(?:\.[a-zA-Z]+)?").IsMatch(Email), "The field Address from E-mail is not a valid E-mail Address");
            Ensure.That<DomainException>(Password.IsBase64(), "The field Password from User is not in Base64 format");
            Ensure.That<DomainException>(!string.IsNullOrEmpty(FullName), "The field FullName from User cannot be created null or empty");
            Ensure.That<DomainException>(UserRoleId != Guid.Empty, "The field UserRoleId from User cannot be empty");
        }
    }
}
