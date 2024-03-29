﻿using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Rent.Data.Repositories;
using Aluguru.Marketplace.Rent.Domain;
using Aluguru.Marketplace.Rent.Dtos;
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

        public async Task<DeleteVoucherCommandResponse> Handle(RemoveVoucherCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Order>();

            var order = await queryRepository.GetOrderAsync(command.OrderId, false);
            
            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The user order Id=[{command.OrderId}] was not found"));
                return default;
            }

            if (order.Voucher == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The user order Id=[{command.OrderId}] does not contain a voucher"));
                return default;
            }

            order.RemoveVoucher();

            var userRepository = _unitOfWork.Repository<Order>();

            order = userRepository.Update(order);

            return new DeleteVoucherCommandResponse()
            {
                Order = _mapper.Map<OrderDTO>(order)
            };
        }
    }
}
