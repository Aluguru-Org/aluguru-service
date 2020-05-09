using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mubbi.Marketplace.Shared.DomainObjects
{
    public abstract class Entity
    {
        protected Entity()
        {            
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        public abstract void ValidateCreation();
    }
}
