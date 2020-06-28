using System;
using System.ComponentModel.DataAnnotations;
using static PampaDevs.Utils.Helpers.DateTimeHelper;

namespace Mubbi.Marketplace.Domain
{
    public interface IEntity : IIdentity
    {
    }

    public abstract class Entity : Identity, IEntity
    {
        protected Entity(Guid id)
        {
            Id = id;
            DateCreated = NewDateTime();
        }
        public DateTime DateCreated { get; protected set; }
        public DateTime? DateUpdated { get; protected set; }

        protected abstract void ValidateCreation();
    }
}
