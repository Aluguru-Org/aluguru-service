using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Register.Dtos;
using Aluguru.Marketplace.Register.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;
using Aluguru.Marketplace.Register.Domain;

namespace Aluguru.Marketplace.Register.Usecases.GetUserById
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

            return new GetUserByIdCommandResponse { User = _mapper.Map<UserDTO>(user) };
        }
    }
}
