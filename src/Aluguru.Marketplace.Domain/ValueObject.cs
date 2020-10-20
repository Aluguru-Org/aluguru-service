using PampaDevs.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aluguru.Marketplace.Domain
{
    public abstract class ValueObject
    {
        protected abstract void ValidateValueObject();
        protected abstract IEnumerable<object> GetEqualityComponents();
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (ReferenceEquals(null, obj))
                return false;
            if (GetType() != obj.GetType())
                return false;
            var vo = obj as ValueObject;
            return GetEqualityComponents().SequenceEqual(vo.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents().CombineHashCodes();
        }
    }
}
