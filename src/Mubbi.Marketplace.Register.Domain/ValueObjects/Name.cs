using Mubbi.Marketplace.Shared.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.Domain.ValueObjects
{
    public class Name : ValueObject
    {
        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

            Validate();
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public override void Validate()
        {
            EntityConcerns.IsEmpty(FirstName, "The field FirstName cannot be empty");
            EntityConcerns.IsEmpty(LastName, "The field LastName cannot be empty");
        }
    }
}
