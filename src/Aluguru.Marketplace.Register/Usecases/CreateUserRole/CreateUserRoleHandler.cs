using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Infrastructure.Data;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Register.Usecases.CreateUserRole
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
                Role = _mapper.Map<UserRoleDTO>(userRole)
            };
        }
    }
}
