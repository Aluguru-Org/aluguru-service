using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Crosscutting.AzureStorage;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static PampaDevs.Utils.Helpers.DateTimeHelper;

namespace Aluguru.Marketplace.Catalog.Usecases.AddProductImage
{
    public class AddProductImageHandler : IRequestHandler<AddProductImageCommand, AddProductImageCommandResponse> 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IAzureStorageGateway _azureStorageGateway;
        public AddProductImageHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler, IAzureStorageGateway azureStorageGateway)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
            _azureStorageGateway = azureStorageGateway;            
        }

        public async Task<AddProductImageCommandResponse> Handle(AddProductImageCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Product>();
            var repository = _unitOfWork.Repository<Product>();

            var product = await queryRepository.GetProductAsync(command.ProductId, false);

            if (product == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"You're trying to upload a image to a product that do not exist"));
                return default;
            }

            foreach(var file in command.Files)
            {                
                var temp = file.FileName.Split('.');

                var fileName = string.Join("", temp.Take(temp.Length - 1));
                var fileExtension = temp[temp.Length - 1];

                var blobName = $"products/{product.Id}/{fileName}_{ToUnixEpochDate(NewDateTime())}.{fileExtension}";

                var url = await _azureStorageGateway.UploadBlob("img", blobName, file);

                if (!string.IsNullOrEmpty(url))
                {
                    product.AddImage(url);
                }
            }

            product = repository.Update(product);

            return new AddProductImageCommandResponse()
            {
                Product = _mapper.Map<ProductDTO>(product)
            };
        }

        private long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
