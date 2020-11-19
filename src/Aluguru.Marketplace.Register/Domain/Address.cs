using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Register.Dtos;
using PampaDevs.Utils;
using System;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Aluguru.Marketplace.Register.Domain
{
    public class Address : Entity
    {
        private Address() : base(NewId()) { }
        public Address(Guid userId, string street, string number, string neighborhood, string city, string state, string country, string zipCode, string complement)
            : base(NewId())
        {
            UserId = userId;
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
            Complement = complement;

            ValidateEntity();
        }
        public Guid UserId { get; private set; }
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }
        public string Complement { get; set; }

        // Ef Relational
        public virtual User User { get; set; }

        internal void UpdateAddress(AddressDTO address)
        {
            Street = address.Street;
            Number = address.Number;
            Neighborhood = address.Neighborhood;
            City = address.City;
            State = address.State;
            Country = address.Country;
            ZipCode = address.ZipCode;
        }

        protected override void ValidateEntity()
        {
            Ensure.NotNullOrEmpty(Street, "The field Street from Address cannot be empty");
            Ensure.NotNullOrEmpty(Number, "The field Street from Number cannot be empty");
            Ensure.NotNullOrEmpty(Neighborhood, "The field Neighborhood from Address cannot be empty");
            Ensure.NotNullOrEmpty(City, "The field City from Address cannot be empty");
            Ensure.NotNullOrEmpty(State, "The field State from Address cannot be empty");
            Ensure.NotNullOrEmpty(Country, "The field Country from Address cannot be empty");
            Ensure.NotNullOrEmpty(ZipCode, "The field ZipCode from Address cannot be empty");
        }


    }
}
