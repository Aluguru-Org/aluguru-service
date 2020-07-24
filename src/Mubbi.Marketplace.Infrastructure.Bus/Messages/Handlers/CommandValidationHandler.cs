using MediatR;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Infrastructure.Bus.Messages.Handlers
{
    public class CommandValidationHandler<TRequest, TRequestResponse> : IPipelineBehavior<TRequest, TRequestResponse>
         where TRequest : IRequest<TRequestResponse>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public CommandValidationHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task<TRequestResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TRequestResponse> next)
        {
            if (IsCommand(request))
            {
                var command = request as Command<TRequestResponse>;

                if (command.IsValid())
                    return await next();

                PublishErrorNotifications(command);

                return default;
            }

            return await next();
        }

        private void PublishErrorNotifications(Command<TRequestResponse> command)
        {
            foreach (var error in command.ValidationResult.Errors)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, error.ErrorMessage));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        private bool IsCommand(TRequest request)
        {
            return request.GetType().BaseType.IsGenericType && request.GetType().BaseType.GetGenericTypeDefinition().IsAssignableFrom(typeof(Command<>));
        }
    }
}
