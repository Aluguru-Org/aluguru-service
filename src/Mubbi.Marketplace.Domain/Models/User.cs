using Mubbi.Marketplace.Domain.Core.Models;
using Mubbi.Marketplace.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mubbi.Marketplace.Domain.Models
{
    [Table("User")]
    public class User : Entity
    {
        public User(Credential credential, Name name, Email email, Document document, Address address)
        {
            Credential = credential;
            Name = name;
            Email = email;
            Document = document;
            Address = address;
        }

        public Credential Credential { get; private set; }
        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public Document Document { get; private set; }
        public Address Address { get; private set; }

        public override void Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}
