using Aluguru.Marketplace.Communication.IntegrationEvents;
using Aluguru.Marketplace.Crosscutting.Iugu;
using Aluguru.Marketplace.Crosscutting.Iugu.Dtos;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Payment.Domain;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Domain.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Payment.Usecases.Charge
{
    public class ChargeHandler : IRequestHandler<ChargeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IIuguService _iuguService;

        public ChargeHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler, IIuguService iuguService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
            _iuguService = iuguService;
        }

        public async Task<bool> Handle(ChargeCommand command, CancellationToken cancellationToken)
        {
            var userQueryRepository = _unitOfWork.QueryRepository<User>();
            var paymentRepository = _unitOfWork.Repository<Domain.Payment>();

            var user = await userQueryRepository.GetUserAsync(command.Order.UserId);

            if (user == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"User {command.Order.UserId} does not exist"));
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

            foreach(var orderItem in command.Order.OrderItems)
            {
                var item = new ItemDTO
                {
                    PriceCents = orderItem.ProductPrice.ToString(),
                    Description = orderItem.ProductName,
                    Quantity = orderItem.Amount.ToString()
                };
                items.Add(item);
            }

            var paymentResponse = await _iuguService.Charge(
                paymentMethod: string.IsNullOrEmpty(command.Token) ? PaymentMethod.BOLETO : PaymentMethod.CREDIT_CARD, 
                command.Token, 1, user.Email, payer, items);

            if (!paymentResponse.Success)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Payment was unsucessfull for order {command.Order.Id}"));
                foreach(var kvp in paymentResponse.Errors)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"{kvp.Key}: {string.Join(", ", kvp.Value)}"));
                }
                return default;
            }

            var paymentMethod = string.IsNullOrEmpty(command.Token) ? EPaymentMethod.BOLETO : EPaymentMethod.CREDIT_CARD;

            var payment = new Domain.Payment(command.Order.UserId, command.Order.Id, paymentMethod, paymentResponse.InvoiceId, paymentResponse.Url, paymentResponse.Pdf, paymentResponse.Identification);

            payment.AddEvent(new PaymentPaidEvent(user.FullName, user.Email, $"/order/{command.Order.Id}", payment.Url, payment.Pdf));

            await paymentRepository.AddAsync(payment);

            return true;
        }
    }
}
