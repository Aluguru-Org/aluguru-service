﻿using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class UpdateUserViewModel : IDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public DocumentViewModel Document { get; set; }
        public ContactViewModel Contact { get; set; }
        public AddressViewModel Address { get; set; }
    }
}
