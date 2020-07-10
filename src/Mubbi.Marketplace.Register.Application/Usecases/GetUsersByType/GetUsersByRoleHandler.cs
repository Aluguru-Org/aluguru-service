using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Register.ViewModels;
using Mubbi.Marketplace.Register.Domain.Models;
using Mubbi.Marketplace.Register.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Register.Usecases.GetUsersByRole
{
    public class GetUsersByRoleHandler : IRequestHandler<GetUsersByRoleCommand, GetUsersByRoleCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUsersByRoleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetUsersByRoleCommandResponse> Handle(GetUsersByRoleCommand request, CancellationToken cancellationToken)
        {
            var userQueryRepository = _unitOfWork.QueryRepository<User>();

            var users = await userQueryRepository.GetUsersByRoleAsync(request.Role);

            return new GetUsersByRoleCommandResponse { Users = _mapper.Map<List<UserViewModel>>(users) };
        }
    }
}
