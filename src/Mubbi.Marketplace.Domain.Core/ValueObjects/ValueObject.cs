using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Domain.Core.ValueObjects
{
    public abstract class ValueObject
    {
        public abstract void Validate();
    }
}
