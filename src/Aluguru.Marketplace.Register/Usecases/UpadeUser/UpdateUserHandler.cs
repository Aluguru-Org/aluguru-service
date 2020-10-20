using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Domain.Repositories;
using Aluguru.Marketplace.Register.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Register.Usecases.UpadeUser
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
