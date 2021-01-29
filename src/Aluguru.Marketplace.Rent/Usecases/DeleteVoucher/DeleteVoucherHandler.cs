using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Infrastructure.Data;
using Aluguru.Marketplace.Rent.Data.Repositories;
using Aluguru.Marketplace.Rent.Domain;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Usecases.DeleteVoucher
{
    public class DeleteVoucherHandler : IRequestHandler<DeleteVoucherCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;

        public DeleteVoucherHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(DeleteVoucherCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Voucher>();

            var voucher = await queryRepository.GetVoucherAsync(request.Code);
            
            if (voucher == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"The user voucher {voucher.Code} was not found"));
                return false;
            }

            if (voucher.Orders.Any())
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"The voucher {voucher.Code} cannot be deleted because it is already in use"));
                return false;
            }

            var repository = _unitOfWork.Repository<Voucher>();

            repository.Delete(voucher);

            return true;
        }
    }
}
