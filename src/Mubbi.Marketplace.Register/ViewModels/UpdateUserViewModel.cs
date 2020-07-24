using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class UpdateUserViewModel : IDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DocumentViewModel Document { get; set; }
        public AddressViewModel Address { get; set; }
    }
}
