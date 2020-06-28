using FluentValidation.Results;
using MediatR;
using System;

namespace Mubbi.Marketplace.Infrastructure.Bus.Messages
{
    public abstract class Command : Message, IRequest
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult;

        protected Command()
        {
            Timestamp = DateTime.UtcNow;
        }

        public abstract bool IsValid();
    }

    public abstract class Command<T> : Message, IRequest<T>
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult;

        protected Command()
        {
            Timestamp = DateTime.UtcNow;
        }

        public abstract bool IsValid();
    }
}
