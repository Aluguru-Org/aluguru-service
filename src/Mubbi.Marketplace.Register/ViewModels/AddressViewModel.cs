using Mubbi.Marketplace.Domain;
using System;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class AddressViewModel : IDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}
