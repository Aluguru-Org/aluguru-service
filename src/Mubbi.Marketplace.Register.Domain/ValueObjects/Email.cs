using Mubbi.Marketplace.Shared.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public Email(string address)
        {
            Address = address;

            Validate();
        }

        public string Address { get; private set; }
        public override void Validate()
        {
            EntityConcerns.IsNotMatch(@"^[a-zA-Z0-9._]+@[a-zA-Z0-9]+\.[a-z]+(?:\.[a-zA-Z]+)?", Address, "The field Address from E-mail is not a valid E-mail Address");
        }
    }
}
