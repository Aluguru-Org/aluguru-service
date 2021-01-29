using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Rent.Data.Repositories;
using Aluguru.Marketplace.Rent.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Usecases.ConfirmOrderPayment
{
    public class ConfirmOrderPaymentHandler : IRequestHandler<ConfirmOrderPaymentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;

        public ConfirmOrderPaymentHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(ConfirmOrderPaymentCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Order>();

            var order = await queryRepository.GetOrderAsync(command.OrderId, false);

            if (order == null)
            {
                return false;
            }

            var repository = _unitOfWork.Repository<Order>();

            order.MarkAsPaid();

            //order.AddEvent()

            repository.Update(order);

            return true;
        }
    }
}
