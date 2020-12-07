using Aluguru.Marketplace.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Aluguru.Marketplace.Rent.Dtos
{
    public class CalculateOrderFreigthDTO : IDto
    {
        [Required]
        public string Number { get; set; }
        [Required]
        public string Complement { get; set; }
        [Required]
        public string ZipCode { get; set; }
    }
}
