using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Register.Domain
{
    public class Address : ValueObject
    {
        public Address(string street, string number, string neighborhood, string city, string state, string country, string zipCode)
        {
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;

            ValidateCreation();
        }

        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Street;
            yield return Number;
            yield return Neighborhood;
            yield return City;
            yield return State;
            yield return Country;
            yield return ZipCode;
        }

        protected override void ValidateCreation()
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
