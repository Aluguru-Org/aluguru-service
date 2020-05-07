using Mubbi.Marketplace.Domain.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public Email(string address)
        {
            Address = address;
        }

        public string Address { get; private set; }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
