using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Register.ViewModels;
using Mubbi.Marketplace.Register.Domain.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mubbi.Marketplace.Register.Domain;
using Mubbi.Marketplace.Register.Data.Repositories;

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
            var userQueryRepository = _unitOfWork.QueryRepository<UserRole>();

            var users = await userQueryRepository.GetUsersByRoleAsync(request.Role);

            return new GetUsersByRoleCommandResponse { Users = _mapper.Map<List<UserViewModel>>(users) };
        }
    }
}
