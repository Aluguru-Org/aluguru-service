using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Rent.Data.Repositories;
using Aluguru.Marketplace.Rent.Domain;
using Aluguru.Marketplace.Rent.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Usecases.ApplyVoucher
{
    public class ApplyVoucherHandler : IRequestHandler<ApplyVoucherCommand, ApplyVoucherCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public ApplyVoucherHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<ApplyVoucherCommandResponse> Handle(ApplyVoucherCommand request, CancellationToken cancellationToken)
        {
            var voucherQueryRepository = _unitOfWork.QueryRepository<Voucher>();
            var orderQueryRepository = _unitOfWork.QueryRepository<Order>();

            var voucher = await voucherQueryRepository.GetVoucherAsync(request.VoucherCode);

            if (voucher == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"Voucher not found"));
                return default;
            }

            var order = await orderQueryRepository.GetOrderAsync(request.OrderId, false);

            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"Order not found"));
                return default;
            }

            var orderRepository = _unitOfWork.Repository<Order>();

            order.ApplyVoucher(voucher);

            order = orderRepository.Update(order);

            return new ApplyVoucherCommandResponse()
            {
                Order = _mapper.Map<OrderViewModel>(order)
            };
        }
    }
}