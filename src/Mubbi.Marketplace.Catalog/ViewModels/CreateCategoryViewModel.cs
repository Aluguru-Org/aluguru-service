﻿using Mubbi.Marketplace.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mubbi.Marketplace.Catalog.ViewModels
{
    public class CreateCategoryViewModel : IDto
    {        
        public Guid? MainCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
