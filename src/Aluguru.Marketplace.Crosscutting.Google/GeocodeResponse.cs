namespace Aluguru.Marketplace.Crosscutting.Google
{
    public class GeocodeResponse
    {
        public GeocodeResponse(bool success)
        {
            Success = success;
        }

        public GeocodeResponse(bool success, string formattedAddress, string street, string number, string neighborhood, string city, string state, string country, string zipCode)
        {
            Success = success;
            FormattedAddress = formattedAddress;
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
        }

        public bool Success { get; private set; }
        public string FormattedAddress { get; set; }
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }
    }
}
