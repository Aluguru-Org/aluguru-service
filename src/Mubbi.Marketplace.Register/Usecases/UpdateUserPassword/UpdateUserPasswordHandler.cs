using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Register.Domain;
using Mubbi.Marketplace.Register.Domain.Repositories;
using Mubbi.Marketplace.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Register.Usecases.UpdateUserPassword
{
    public class UpdateUserPasswordHandler : IRequestHandler<UpdateUserPasswordCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;

        public UpdateUserPasswordHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(UpdateUserPasswordCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<User>();

            if (command.CurrentUserId != command.UserId)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, "You can only change your own password"));
                return false;
            }

            var user = await queryRepository.GetUserAsync(command.UserId);

            if (user == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The User with Id [{command.UserId}] does not exist"));
                return false;
            }

            if (user.Password == Cryptography.Encrypt(command.Password))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The new password must be different from the previous one"));
                return false;
            }

            var repository = _unitOfWork.Repository<User>();

            user.UpdatePassword(command.Password);

            repository.Update(user);

            return true;
        }
    }
}
