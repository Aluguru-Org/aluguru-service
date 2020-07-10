using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using static PampaDevs.Utils.Helpers.DateTimeHelper;

namespace Mubbi.Marketplace.Domain
{
    public interface IAggregateRoot : IEntity
    {
        IAggregateRoot ApplyEvent(Event payload);
        List<Event> GetUncommittedEvents();
        void ClearUncommittedEvents();
        IAggregateRoot RemoveEvent(Event @event);
        IAggregateRoot AddEvent(Event uncommittedEvent);
        IAggregateRoot RegisterHandler<T>(Action<T> handler);
    }

    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        private readonly IDictionary<Type, Action<object>> _handlers = new ConcurrentDictionary<Type, Action<object>>();
        private readonly List<Event> _uncommittedEvents = new List<Event>();
        protected AggregateRoot() : this(default)
        {
        }

        protected AggregateRoot(Guid id) : base(id)
        {
        }

        public IAggregateRoot AddEvent(Event uncommittedEvent)
        {
            _uncommittedEvents.Add(uncommittedEvent);
            ApplyEvent(uncommittedEvent);
            return this;
        }

        public IAggregateRoot ApplyEvent(Event payload)
        {
            if (!_handlers.ContainsKey(payload.GetType()))
                return this;
            _handlers[payload.GetType()]?.Invoke(payload);
            return this;
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        public List<Event> GetUncommittedEvents()
        {
            return _uncommittedEvents;
        }

        public IAggregateRoot RegisterHandler<T>(Action<T> handler)
        {
            _handlers.Add(typeof(T), e => handler((T)e));
            return this;
        }

        public IAggregateRoot RemoveEvent(Event @event)
        {
            if (_uncommittedEvents.Find(e => e == @event) != null)
                _uncommittedEvents.Remove(@event);
            return this;
        }
    }
}
