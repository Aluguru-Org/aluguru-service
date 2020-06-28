using System;
using System.ComponentModel.DataAnnotations;

namespace Mubbi.Marketplace.Domain
{
    public interface IIdentity
    {
        Guid Id { get; }
    }

    public abstract class Identity : IEquatable<Identity>, IIdentity
    {
        [Key]
        public Guid Id { get; protected set; }

        protected Identity()
        {
            Id = Id;
        }

        public bool Equals(Identity other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (ReferenceEquals(null, other))
                return false;

            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Identity);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() ^ 93 + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }
    }
}
