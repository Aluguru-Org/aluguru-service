﻿using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Register.Domain
{
    public class Name : ValueObject
    {
        public Name(string firstName, string lastName, string fullName)
        {
            FirstName = firstName;
            LastName = lastName;
            FullName = FullName;

            ValidateValueObject();
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
            yield return FullName;
        }

        protected override void ValidateValueObject()
        {
            Ensure.NotNullOrEmpty(FirstName, "The field FirstName cannot be empty");
            Ensure.NotNullOrEmpty(LastName, "The field LastName cannot be empty");
            Ensure.NotNullOrEmpty(FullName, "The field FullName cannot be empty");
        }
    }
}
