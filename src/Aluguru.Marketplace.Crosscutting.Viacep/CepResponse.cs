using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Crosscutting.Viacep
{
    public class CepResponse
    {
        public CepResponse(bool success)
        {
            Success = success;
        }

        public CepResponse(bool success, string street, string neighborhood, string city, string state, string zipCode)
        {
            Success = success;
            Street = street;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public bool Success { get; private set; }
        public string Street { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }        
        public string ZipCode { get; private set; }         
    }
}
