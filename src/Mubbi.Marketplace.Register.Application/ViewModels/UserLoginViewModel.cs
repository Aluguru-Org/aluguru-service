﻿using System.ComponentModel.DataAnnotations;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class UserLoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
