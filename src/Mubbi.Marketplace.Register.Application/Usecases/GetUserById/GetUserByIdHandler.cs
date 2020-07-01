using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Data;
using Mubbi.Marketplace.Register.Application.ViewModels;
using Mubbi.Marketplace.Register.Domain.Models;
using Mubbi.Marketplace.Register.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Register.Application.Usecases.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdCommand, GetUserByIdCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetUserByIdCommandResponse> Handle(GetUserByIdCommand command, CancellationToken cancellationToken)
        {
            var userRepository = _unitOfWork.QueryRepository<User>();

            var user = await userRepository.GetUserAsync(command.UserId);

            return new GetUserByIdCommandResponse { User = _mapper.Map<UserViewModel>(user) };
        }
    }
}
