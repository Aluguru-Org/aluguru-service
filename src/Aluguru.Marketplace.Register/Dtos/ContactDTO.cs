﻿using Aluguru.Marketplace.Domain;
using System;

namespace Aluguru.Marketplace.Register.Dtos
{
    public class ContactDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
