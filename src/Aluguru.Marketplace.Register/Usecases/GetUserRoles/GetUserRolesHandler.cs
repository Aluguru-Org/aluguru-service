using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Register.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aluguru.Marketplace.Register.Data.Repositories;

namespace Aluguru.Marketplace.Register.Usecases.GetUserRoles
{
    public class GetUserRolesHandler : IRequestHandler<GetUserRolesCommand, GetUserRolesCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserRolesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetUserRolesCommandResponse> Handle(GetUserRolesCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<UserRole>();

            var roles = await queryRepository.GetUserRolesAsync();

            return new GetUserRolesCommandResponse() { UserRoles = _mapper.Map<List<UserRole>>(roles) };
        }
    }
}