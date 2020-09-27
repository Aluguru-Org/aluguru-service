using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Catalog.Data.Repositories;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Crosscutting.AzureStorage;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static PampaDevs.Utils.Helpers.DateTimeHelper;

namespace Mubbi.Marketplace.Catalog.Usecases.AddProductImage
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
                var fileName = $"{product.Id}_{file.Name}_{NewDateTime()}";
                var url = await _azureStorageGateway.UploadFile("img", fileName, file);

                if (!string.IsNullOrEmpty(url))
                {
                    product.AddImage(url);
                }
            }

            product = repository.Update(product);

            return new AddProductImageCommandResponse()
            {
                Product = _mapper.Map<ProductViewModel>(product)
            };
        }
    }
}
