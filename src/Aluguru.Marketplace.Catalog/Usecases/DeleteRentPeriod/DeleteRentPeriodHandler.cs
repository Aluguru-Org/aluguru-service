using MediatR;
using Microsoft.EntityFrameworkCore;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Usecases.DeleteRentPeriod
{
    public class DeleteRentPeriodHandler : IRequestHandler<DeleteRentPeriodCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;

        public DeleteRentPeriodHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(DeleteRentPeriodCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<RentPeriod>();

            var rentPeriod = await queryRepository.GetByIdAsync(command.RentPeriodId);

            if (rentPeriod == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The rent period {rentPeriod} was not found"));
                return false;
            }

            var repository = _unitOfWork.Repository<RentPeriod>();

            repository.Delete(rentPeriod);

            return true;
        }
    }
}
