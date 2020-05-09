using FluentValidation.Results;
using MediatR;
using System;

namespace Mubbi.Marketplace.Shared.Messages
{
    public abstract class Command : Message, IRequest<bool>
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult;

        protected Command()
        {
            Timestamp = DateTime.UtcNow;
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
