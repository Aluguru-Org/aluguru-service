using MediatR;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Usecases.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;

        public DeleteProductHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Product>();

            var product = await queryRepository.GetByIdAsync(command.ProductId);

            if (product == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The product {product} was not found"));
                return false;
            }

            var repository = _unitOfWork.Repository<Product>();

            repository.Delete(product);

            return true;
        }
    }
}
