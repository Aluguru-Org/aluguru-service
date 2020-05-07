﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Domain.Core.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException()
        { }

        public DomainException(string message) : base(message)
        { }

        public DomainException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
