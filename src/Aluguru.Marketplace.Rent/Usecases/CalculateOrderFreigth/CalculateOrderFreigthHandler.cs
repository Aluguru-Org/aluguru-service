using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Crosscutting.Google;
using Aluguru.Marketplace.Crosscutting.Viacep;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Domain.Repositories;
using Aluguru.Marketplace.Rent.Data.Repositories;
using Aluguru.Marketplace.Rent.Domain;
using Aluguru.Marketplace.Rent.Dtos;
using Aluguru.Marketplace.Rent.Utils;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Usecases.CalculateOrderFreigth
{
    public class CalculateOrderFreigthHandler : IRequestHandler<CalculateOrderFreigthCommand, CalculateOrderFreigthCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ICepService _cepService;
        private readonly IDistanceMatrixService _distanceMatrixService;

        public CalculateOrderFreigthHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler, ICepService cepService, IDistanceMatrixService distanceMatrixService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
            _cepService = cepService;
            _distanceMatrixService = distanceMatrixService;
        }

        public async Task<CalculateOrderFreigthCommandResponse> Handle(CalculateOrderFreigthCommand command, CancellationToken cancellationToken)
        {
            var userQueryRepository = _unitOfWork.QueryRepository<User>();
            var orderQueryRepository = _unitOfWork.QueryRepository<Order>();
            var productQueryRepository = _unitOfWork.QueryRepository<Product>();

            var order = await orderQueryRepository.GetOrderAsync(command.OrderId, false);

            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Order not found"));
                return default;
            }

            if (order.UserId != command.UserId)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Order can only be edited by order owner"));
                return default;
            }

            var address = await _cepService.GetAddress(command.ZipCode);

            if (!address.Success)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Invalid ZipCode provided"));
                return default;
            }

            order.UpdateDeliveryAddress($"{address.Street}, {command.Number}, {command.Complement} - {address.Neighborhood}, {address.City} - {address.State}, {address.ZipCode}");

            foreach (var orderItem in order.OrderItems)
            {
                var product = await productQueryRepository.GetProductAsync(orderItem.ProductId);

                if (product == null)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The product {product.Id} was not found in catalog."));
                    continue;
                }

                var owner = await userQueryRepository.GetUserAsync(product.UserId);

                if (owner == null)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The product {product.Id} owner was not found in register."));
                    continue;
                }

                var distanceMatrixResponse = await _distanceMatrixService.Distance(owner.Address.ToString(), order.DeliveryAddress);

                if (!distanceMatrixResponse.Success)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The User address or Company address was not found. Please, contact support."));
                    continue;
                }

                var freigthPrice = RentUtils.CalculateProductFreigthPrice(product, distanceMatrixResponse.Distance);

                order.UpdateItemFreigthPrice(orderItem.Id, freigthPrice);
            }

            order.MarkAsFreigthCalculated();

            var orderRepository = _unitOfWork.Repository<Order>();

            order = orderRepository.Update(order);

            return new CalculateOrderFreigthCommandResponse()
            {
                Order = _mapper.Map<OrderDTO>(order)
            };
        }
    }
}
