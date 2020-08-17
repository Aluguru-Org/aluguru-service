using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Catalog.Data.Repositories;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Rent.Domain;
using Mubbi.Marketplace.Rent.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Rent.Usecases.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public CreateOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var productQueryRepository = _unitOfWork.QueryRepository<Product>();

            var productIds = request.OrderItems.Select(x => x.ProductId).ToList();
            var products = await productQueryRepository.GetProductsAsync(productIds);

            if (products.Count == 0)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"No product was found"));
                return new CreateOrderCommandResponse();
            }

            var orderRepository = _unitOfWork.Repository<Order>();

            var order = new Order(request.UserId);
            
            foreach(var orderItem in request.OrderItems)
            {
                var product = products.FirstOrDefault(x => x.Id == orderItem.ProductId);

                // What should be the correct behaviour when the product is not found?
                if (product == null) continue;

                decimal price = 0;

                switch(product.RentType)
                {
                    case ERentType.Fixed:
                        price = orderItem.RentDays * product.Price.GetDailyRentPrice();
                        break;
                    case ERentType.Indefinite:
                        price = product.Price.GetPeriodRentPrice(orderItem.SelectedRentPeriod.Value); 
                        break;
                }

                var newOrderItem = new OrderItem(product.Id, product.Name, orderItem.Amount ?? 1, price);
                order.AddItem(newOrderItem);
            }

            order = await orderRepository.AddAsync(order);

            return new CreateOrderCommandResponse()
            {
                Order = _mapper.Map<OrderViewModel>(order)
            };
        }
    }
}