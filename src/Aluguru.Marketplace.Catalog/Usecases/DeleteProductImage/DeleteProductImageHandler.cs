using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Catalog.ViewModels;
using Aluguru.Marketplace.Crosscutting.AzureStorage;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Usecases.DeleteProductImage
{
    public class DeleteProductImageHandler : IRequestHandler<DeleteProductImageCommand, DeleteProductImageCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IAzureStorageGateway _azureStorageGateway;
        public DeleteProductImageHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler, IAzureStorageGateway azureStorageGateway)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
            _azureStorageGateway = azureStorageGateway;
        }

        public async Task<DeleteProductImageCommandResponse> Handle(DeleteProductImageCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Product>();
            var repository = _unitOfWork.Repository<Product>();

            var product = await queryRepository.GetProductAsync(command.ProductId, false);

            if (product == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"You're trying to delete a image from a product that do not exist"));
                return default;
            }

            foreach(var imageUrl in command.ImageUrls)
            {
                var array = imageUrl.Split('/');
                var fileName = array[array.Length -1];

                await _azureStorageGateway.DeleteBlob("img", fileName);
                product.RemoveImage(imageUrl);
            }

            product = repository.Update(product);

            return new DeleteProductImageCommandResponse()
            {
                Product = _mapper.Map<ProductViewModel>(product)
            };
        }
    }
}
