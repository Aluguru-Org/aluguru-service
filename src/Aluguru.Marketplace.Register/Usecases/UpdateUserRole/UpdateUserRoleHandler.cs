using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Register.Data.Repositories;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Register.Usecases.UpdateUserRole
{
    public class UpdateUserRoleHandler : IRequestHandler<UpdateUserRoleCommand, UpdateUserRoleCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public UpdateUserRoleHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<UpdateUserRoleCommandResponse> Handle(UpdateUserRoleCommand command, CancellationToken cancellationToken)
        {
            if (command.UserRoleId != command.UserRole.Id)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The provided UserRole Id [{command.UserRoleId}] does not match with the UserRole passed to be updated [{command.UserRole.Id}]"));
                return new UpdateUserRoleCommandResponse();
            }

            var queryRepository = _unitOfWork.QueryRepository<UserRole>();

            var existed = await queryRepository.GetUserRoleAsync(command.UserRoleId, false);

            if (existed == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The UserRole with Id [{command.UserRoleId}] does not exist"));
                return new UpdateUserRoleCommandResponse();
            }

            var repository = _unitOfWork.Repository<UserRole>();

            var updated = existed.UpdateUserRole(command);
            var userRole = repository.Update(updated);

            return new UpdateUserRoleCommandResponse()
            {
                UserRole = _mapper.Map<UserRoleViewModel>(userRole)
            };
        }
    }
}
