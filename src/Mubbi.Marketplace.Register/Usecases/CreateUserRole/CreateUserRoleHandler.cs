﻿using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Infrastructure.Data;
using Mubbi.Marketplace.Register.Domain;
using Mubbi.Marketplace.Register.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Register.Usecases.CreateUserRole
{
    public class CreateUserRoleHandler : IRequestHandler<CreateUserRoleCommand, CreateUserRoleCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public CreateUserRoleHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<CreateUserRoleCommandResponse> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<UserRole>();

            if (await queryRepository.FindOneAsync(x => x.Name == request.Name) != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"The Role {request.Name} already exists"));
                return new CreateUserRoleCommandResponse();
            }

            var repository = _unitOfWork.Repository<UserRole>();

            var userRole = new UserRole(request.Name);

            userRole = await repository.AddAsync(userRole);

            return new CreateUserRoleCommandResponse()
            {
                Role = _mapper.Map<UserRoleViewModel>(userRole)
            };
        }
    }
}