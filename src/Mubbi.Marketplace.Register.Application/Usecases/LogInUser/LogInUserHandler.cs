using MediatR;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Register.Application.Usecases.LogInUser
{
    public class LogInUserHandler : IRequestHandler<LogInUserCommand, LogInUserCommandResponse>
    {
        public async Task<LogInUserCommandResponse> Handle(LogInUserCommand command, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}