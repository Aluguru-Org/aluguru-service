using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Register.Domain;
using Mubbi.Marketplace.Register.Domain.Repositories;
using Mubbi.Marketplace.Register.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Register.Usecases.UpadeUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public UpdateUserHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<User>();

            var user = await queryRepository.GetUserAsync(command.UserId, false);
            
            if (user == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The User with Id [{command.UserId}] does not exist"));
                return new UpdateUserCommandResponse();
            }

            var repository = _unitOfWork.Repository<User>();

            user.UpdateUser(command);
            repository.Update(user);

            return new UpdateUserCommandResponse()
            {
                User = _mapper.Map<UserViewModel>(user)
            };
        }
    }
}
