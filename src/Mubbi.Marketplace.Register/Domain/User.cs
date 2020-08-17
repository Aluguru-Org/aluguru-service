﻿using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure;
using Mubbi.Marketplace.Register.Usecases.UpadeUser;
using PampaDevs.Utils;
using System;
using System.Text.RegularExpressions;
using Mubbi.Marketplace.Register.Events;
using static PampaDevs.Utils.Helpers.IdHelper;
using static PampaDevs.Utils.Helpers.DateTimeHelper;
using System.Collections.Generic;
using Mubbi.Marketplace.Register.ViewModels;

namespace Mubbi.Marketplace.Register.Domain
{
    public class User : AggregateRoot
    {
        private readonly List<Contact> _contacts;
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
        public IReadOnlyCollection<Contact> Contacts { get { return _contacts; } }
        public Document Document { get; set; }
        public Address Address { get; set; }
        // EF Relational
        public virtual UserRole UserRole { get; set; }

        public User UpdateUser(UpdateUserCommand command)
        {
            FullName = command.FullName;            

            UpdateDocument(command.Document);

            UpdateAddress(command.Address);

            ValidateEntity();

            DateUpdated = NewDateTime();

            AddEvent(new UserUpdatedEvent(Id, this));

            return this;
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
