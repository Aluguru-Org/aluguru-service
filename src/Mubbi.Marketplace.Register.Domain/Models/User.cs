using Microsoft.AspNetCore.Identity;
using Mubbi.Marketplace.Register.Domain.Enums;
using Mubbi.Marketplace.Register.Domain.ValueObjects;
using Mubbi.Marketplace.Shared.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.Domain.Models
{
    public class User : IdentityUser
    {
        public string Username { get; private set; }
        public int Password { get; private set; }
        public ERoles Role { get; private set; }        
        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public Address Address { get; private set; }
        public Document Document { get; private set; }

        public void ValidateCreation()
        {
            EntityConcerns.IsEmpty(Username, "The field Username from User cannot be empty");
            EntityConcerns.IsEqual(ERoles.Admin, Role, "The field Role from User cannot be created as Admin");
        }
    }
}
