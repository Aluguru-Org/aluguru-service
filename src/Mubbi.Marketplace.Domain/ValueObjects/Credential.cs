using Mubbi.Marketplace.Domain.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Domain.ValueObjects
{
    public class Credential : ValueObject
    {
        public Credential(string username, string password)
        {
            Username = username;
            Password = password;
            Role = "user";
        }

        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
