using MediatR;
using Mubbi.Marketplace.Shared.Communication;
using Mubbi.Marketplace.Shared.Messages;
using Mubbi.Marketplace.Shared.Messages.Notifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Register.Application.Usecases.LogInUser
{
    public class LogInUserHandler : IRequestHandler<LogInUserCommand, LogInUserCommandResponse>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public LogInUserHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task<LogInUserCommandResponse> Handle(LogInUserCommand command, CancellationToken cancellationToken)
        {
            if (IsValidCommand(command)) return new LogInUserCommandResponse() { Success = false };


        }

        private bool IsValidCommand<T>(Command<T> command) where T : class
        {
            if (command.IsValid()) return true;

            foreach (var error in command.ValidationResult.Errors)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, error.ErrorMessage));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }

            return false;
        }
    }
}
