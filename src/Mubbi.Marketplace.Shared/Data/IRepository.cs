using Mubbi.Marketplace.Shared.DomainObjects;
using System;

namespace Mubbi.Marketplace.Shared.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get;}
    }
}
