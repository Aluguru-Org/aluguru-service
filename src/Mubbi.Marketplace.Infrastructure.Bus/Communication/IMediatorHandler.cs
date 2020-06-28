using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Infrastructure.Bus.Communication
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T @event) where T : Event;
        Task SendCommand<T>(T command) where T : Command;
        Task<TResponse> SendCommand<T, TResponse>(T command) where T : Command<TResponse>;
        Task PublishNotification<T>(T notification) where T : DomainNotification;
    }
}
