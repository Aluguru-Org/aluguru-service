using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure;
using PampaDevs.Utils;
using System;
using System.Text.RegularExpressions;
using static PampaDevs.Utils.Helpers.IdHelper;
using static PampaDevs.Utils.Helpers.DateTimeHelper;

namespace Aluguru.Marketplace.Register.Domain
{
    public class User : AggregateRoot
    {
        private User() { }

        public User(Guid id, string email, string password, string fullName, Guid role, string activationHash) 
            : base(id)
        {
            Password = password;
            FullName = fullName;
            Email = email;
            UserRoleId = role;
            ActivationHash = activationHash;

            ValidateEntity();
        }

        public User(string email, string password, string fullName, Guid role, string activationHash)
            : base(NewId())
        {
            Password = password;
            FullName = fullName;
            Email = email;
            UserRoleId = role;
            ActivationHash = activationHash;

            ValidateEntity();
        }
        public bool IsActive { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string FullName { get; private set; }
        public Guid UserRoleId { get; private set; }
        public string ActivationHash { get; private set; }
        public Contact Contact { get; set; }
        public Document Document { get; set; }
        public Address Address { get; set; }
        // EF Relational
        public virtual UserRole UserRole { get; set; }

        public bool Activate(string activationHash)
        {
            IsActive = ActivationHash == activationHash;
            return IsActive;
        }

        public User UpdateUserName(string fullName)
        {
            Ensure.That<DomainException>(!string.IsNullOrEmpty(fullName), "The field FullName from User cannot be created null or empty");

            FullName = fullName;        

            ValidateEntity();

            DateUpdated = NewDateTime();

            return this;
        }        

        public void UpdatePassword(string newPassword)
        {
            Ensure.That<DomainException>(newPassword.IsBase64(), "The field Password from User is not in Base64 format");

            Password = newPassword;
        }
        
        public void UpdateDocument(Document document)
        {
            if (document == null) return;

            if (Document == null)
            {
                document.AssignUser(Id);
                Document = document;
            }
            else
            {
                Document.UpdateDocument(document.DocumentType, document.Number);
            }
        }

        public void UpdateContact(Contact contact)
        {
            if (contact == null) return;

            if (Contact == null)
            {
                contact.AssignUser(Id);
                Contact = contact;
            }
            else
            {
                Contact.UpdateContact(contact.Name, contact.PhoneNumber, contact.Email);
            }
        }

        public void UpdateAddress(Address address)
        {
            if (address == null) return;

            if (Address == null)
            {
                address.AssignUser(Id);
                Address = address;
            }
            else
            {
                Address.UpdateAddress(address);
            }
        }

        protected override void ValidateEntity()
        {
            Ensure.That<DomainException>(new Regex(@"^[a-zA-Z0-9._]+@[a-zA-Z0-9]+\.[a-z]+(?:\.[a-zA-Z]+)?").IsMatch(Email), "The field Address from E-mail is not a valid E-mail Address");
            Ensure.That<DomainException>(Password.IsBase64(), "The field Password from User is not in Base64 format");
            Ensure.That<DomainException>(!string.IsNullOrEmpty(FullName), "The field FullName from User cannot be created null or empty");
            Ensure.That<DomainException>(UserRoleId != Guid.Empty, "The field UserRoleId from User cannot be empty");
            Ensure.That<DomainException>(!string.IsNullOrEmpty(ActivationHash), "The field ActivationHash from User cannot be created null or empty");
        }
    }
}
