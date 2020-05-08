using Mubbi.Marketplace.Shared.DomainObjects;

namespace Mubbi.Marketplace.Register.Domain.ValueObjects
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

            Validate();
        }

        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }

        public override void Validate()
        {
            EntityConcerns.IsEmpty(Street, "The field Street from Address cannot be empty");
            EntityConcerns.IsEmpty(Number, "The field Street from Number cannot be empty");
            EntityConcerns.IsEmpty(Neighborhood, "The field Neighborhood from Address cannot be empty");
            EntityConcerns.IsEmpty(City, "The field City from Address cannot be empty");
            EntityConcerns.IsEmpty(State, "The field State from Address cannot be empty");
            EntityConcerns.IsEmpty(Country, "The field Country from Address cannot be empty");
            EntityConcerns.IsEmpty(ZipCode, "The field ZipCode from Address cannot be empty");
        }
    }
}
