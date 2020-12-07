using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Crosscutting.Google;
using Aluguru.Marketplace.Crosscutting.Viacep;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Domain.Repositories;
using Aluguru.Marketplace.Rent.Utils;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Usecases.OrderPreview
{
    public class OrderPreviewHandler : IRequestHandler<OrderPreviewCommand, OrderPreviewCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ICepService _cepService;
        private readonly IDistanceMatrixService _distanceMatrixService;

        public OrderPreviewHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler, ICepService cepService, IDistanceMatrixService distanceMatrixService)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
            _cepService = cepService;
            _distanceMatrixService = distanceMatrixService;
        }

        public async Task<OrderPreviewCommandResponse> Handle(OrderPreviewCommand command, CancellationToken cancellationToken)
        {
            var userQueryRepository = _unitOfWork.QueryRepository<User>();
            var productQueryRepository = _unitOfWork.QueryRepository<Product>();

            var address = await _cepService.GetAddress(command.Preview.ZipCode);

            if (!address.Success)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Invalid ZipCode provided"));
                return default;
            }

            for (int i = 0; i < command.Preview.Items.Count; i ++)
            {
                var item = command.Preview.Items[i];
                var product = await productQueryRepository.GetProductAsync(item.ProductId);

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

                var distanceMatrixResponse = await _distanceMatrixService.Distance(owner.Address.ToString(), $"{address.Street} - {address.Neighborhood}, {address.City} - {address.State}, {address.ZipCode}");

                if (!distanceMatrixResponse.Success)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The User address or Company address was not found. Please, contact support."));
                    continue;
                }

                item.Freigth = RentUtils.CalculateProductFreigthPrice(product, distanceMatrixResponse.Distance);
            }

            command.Preview.TotalFreigth = command.Preview.Items.Sum(x => x.Freigth);

            return new OrderPreviewCommandResponse()
            {
                Order = command.Preview
            };
        }
    }
}
