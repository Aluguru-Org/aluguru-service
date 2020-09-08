using Mubbi.Marketplace.Domain;
using System;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class ContactViewModel : IDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
