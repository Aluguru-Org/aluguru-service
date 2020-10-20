using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure;
using Aluguru.Marketplace.Register.Usecases.UpadeUser;
using PampaDevs.Utils;
using System;
using System.Text.RegularExpressions;
using Aluguru.Marketplace.Register.Events;
using static PampaDevs.Utils.Helpers.IdHelper;
using static PampaDevs.Utils.Helpers.DateTimeHelper;
using Aluguru.Marketplace.Register.ViewModels;

namespace Aluguru.Marketplace.Register.Domain
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
        public bool IsActive { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string FullName { get; private set; }
        public Guid UserRoleId { get; private set; }
        public Contact Contact { get; private set; }
        public Document Document { get; set; }
        public Address Address { get; set; }
        // EF Relational
        public virtual UserRole UserRole { get; set; }

        public void Activate() => IsActive = true;

        public void Deactivate() => IsActive = false;

        public User UpdateUser(UpdateUserCommand command)
        {
            FullName = command.FullName;            

            UpdateDocument(command.Document);

            UpdateContact(command.Contact);

            UpdateAddress(command.Address);

            ValidateEntity();

            DateUpdated = NewDateTime();

            AddEvent(new UserUpdatedEvent(Id, this));

            return this;
        }

        public void UpdatePassword(string newPassword)
        {
            Ensure.That<DomainException>(newPassword.IsBase64(), "The field Password from User is not in Base64 format");

            Password = newPassword;
        }

        private void UpdateDocument(DocumentViewModel document)
        {
            if (document == null) return;

            if (Document == null)
            {
                var newDocument = new Document(document.Number, (EDocumentType)Enum.Parse(typeof(EDocumentType), document.DocumentType));
                newDocument.AssignUser(Id);
                Document = newDocument;
            }
            else
            {
                Document.UpdateDocument((EDocumentType)Enum.Parse(typeof(EDocumentType), document.DocumentType), document.Number);
            }
        }

        private void UpdateContact(ContactViewModel contact)
        {
            if (contact == null) return;

            if (Contact == null)
            {
                var newContact = new Contact(contact.Name, contact.PhoneNumber, contact.Email);
                newContact.AssignUser(Id);
                Contact = newContact;
            }
            else
            {
                Contact.UpdateContact(contact.Name, contact.PhoneNumber, contact.Email);
            }
        }

        private void UpdateAddress(AddressViewModel address)
        {
            if (Address == null)
            {
                var newAddress = new Address(Id, address.Street, address.Number, address.Neighborhood, address.City, address.State, address.Country, address.ZipCode);                
                Address = newAddress;
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
        }
    }
}
