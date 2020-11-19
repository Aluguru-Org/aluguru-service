using Aluguru.Marketplace.Communication.IntegrationEvents;
using Aluguru.Marketplace.Crosscutting.Iugu;
using Aluguru.Marketplace.Crosscutting.Iugu.Dtos;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Payment.Domain;
using Aluguru.Marketplace.Payment.Dtos;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Domain.Repositories;
using Aluguru.Marketplace.Rent.Data.Repositories;
using Aluguru.Marketplace.Rent.Domain;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Payment.Usecases.PayOrder
{
    public class PayOrderHandler : IRequestHandler<PayOrderCommand, PayOrderCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IIuguService _iuguService;

        public PayOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler, IIuguService iuguService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
            _iuguService = iuguService;
        }

        public async Task<PayOrderCommandResponse> Handle(PayOrderCommand command, CancellationToken cancellationToken)
        {
            var userQueryRepository = _unitOfWork.QueryRepository<User>();
            var orderQueryRepository = _unitOfWork.QueryRepository<Order>();

            var paymentRepository = _unitOfWork.Repository<Domain.Payment>();

            var user = await userQueryRepository.GetUserAsync(command.UserId);
            var order = await orderQueryRepository.GetOrderAsync(command.OrderId);

            if (user == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"User {command.UserId} does not exist"));
                return default;
            }

            if (user.Address == null || user.Document == null || user.Contact == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"User {command.UserId} does not have a Address, Contact or Document registered"));
                return default;
            }

            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Order {command.OrderId} does not exist"));
                return default;
            }

            if (order.UserId != command.UserId)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Order {command.OrderId} does not belong to User {command.UserId}"));
                return default;
            }

            if (order.OrderStatus != EOrderStatus.Initiated)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Order {command.OrderId} is not in status Initiated. Initiate the order first."));
                return default;
            }

            var payer = new PayerDTO
            {
                CpfCnpj = user.Document.Number,
                Email = user.Email,
                Name = user.FullName,
                PhonePrefix = user.Contact.PhoneNumber.Substring(0, 2),
                Phone = user.Contact.PhoneNumber.Substring(2)
            };

            var items = new List<ItemDTO>();

            foreach (var orderItem in order.OrderItems)
            {
                var item = new ItemDTO
                {
                    PriceCents = (int)orderItem.ProductPrice,
                    Description = orderItem.ProductName,
                    Quantity = (int)orderItem.Amount
                };
                items.Add(item);
            }

            var paymentResponse = await _iuguService.Charge(
                paymentMethod: string.IsNullOrEmpty(command.Token) ? PaymentMethod.BOLETO : PaymentMethod.CREDIT_CARD,
                command.Token, command.Installments ?? 1, user.Email, payer, items);

            if (!paymentResponse.Success)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Payment was unsucessfull for order {command.OrderId}"));
                foreach (var kvp in paymentResponse.Errors)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"{kvp.Key}: {string.Join(", ", kvp.Value)}"));
                }
                return default;
            }

            var paymentMethod = string.IsNullOrEmpty(command.Token) ? EPaymentMethod.BOLETO : EPaymentMethod.CREDIT_CARD;

            var payment = new Domain.Payment(command.UserId, command.OrderId, paymentMethod, paymentResponse.InvoiceId, paymentResponse.Url, paymentResponse.Pdf, paymentResponse.Identification);

            payment.AddEvent(new ProcessingPaymentEvent(user.FullName, user.Email, $"/conta/pedidos/{command.OrderId}", payment.Url, payment.Pdf));

            await paymentRepository.AddAsync(payment);

            return new PayOrderCommandResponse
            {
                Payment = _mapper.Map<PaymentDTO>(payment)
            };
        }
    }
}
