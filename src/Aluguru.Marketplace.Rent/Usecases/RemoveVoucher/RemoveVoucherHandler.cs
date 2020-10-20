using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Rent.Data.Repositories;
using Aluguru.Marketplace.Rent.Domain;
using Aluguru.Marketplace.Rent.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Usecases.RemoveVoucher
{
    public class RemoveVoucherHandler : IRequestHandler<RemoveVoucherCommand, DeleteVoucherCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMapper _mapper;

        public RemoveVoucherHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
            _mapper = mapper;
        }

        public async Task<DeleteVoucherCommandResponse> Handle(RemoveVoucherCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Order>();

            var order = await queryRepository.GetOrderAsync(request.OrderId);
            
            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"The user order Id=[{request.OrderId}] was not found"));
                return default;
            }

            if (order.Voucher == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"The user order Id=[{request.OrderId}] does not contain a voucher"));
                return default;
            }

            order.RemoveVoucher();

            var userRepository = _unitOfWork.Repository<Order>();

            userRepository.Delete(order);

            return new DeleteVoucherCommandResponse()
            {
                Order = _mapper.Map<OrderViewModel>(order)
            };
        }
    }
}
