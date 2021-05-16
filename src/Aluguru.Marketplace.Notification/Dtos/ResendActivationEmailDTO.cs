using Aluguru.Marketplace.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Aluguru.Marketplace.Notification.Dtos
{
    public class ResendActivationEmailDTO : IDto
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
