using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Aluguru.Marketplace.Register.Usecases.UpadeUserDocument
{
    public class UpdateUserDocumentHandler : IRequestHandler<UpdateUserDocumentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;

        public UpdateUserDocumentHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(UpdateUserDocumentCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<User>();

            var user = await queryRepository.GetUserAsync(command.UserId, false);
            
            if (user == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The User with Id [{command.UserId}] does not exist"));
                return false;
            }

            var repository = _unitOfWork.Repository<User>();
            
            var document = new Document(command.Number, (EDocumentType) Enum.Parse(typeof(EDocumentType), command.DocumentType));
            user.UpdateDocument(document);
            repository.Update(user);

            return true;
        }
    }
}
