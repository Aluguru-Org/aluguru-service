using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Crosscutting.AzureStorage;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using AutoMapper;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static PampaDevs.Utils.Helpers.DateTimeHelper;

namespace Aluguru.Marketplace.Catalog.Usecases.AddCategoryImage
{
    public class UpdateCategoryImageHandler : IRequestHandler<UpdateCategoryImageCommand, UpdateCategoryImageCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IAzureStorageGateway _azureStorageGateway;
        public UpdateCategoryImageHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler, IAzureStorageGateway azureStorageGateway)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
            _azureStorageGateway = azureStorageGateway;
        }

        public async Task<UpdateCategoryImageCommandResponse> Handle(UpdateCategoryImageCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Category>();
            var repository = _unitOfWork.Repository<Category>();

            var category = await queryRepository.GetCategoryAsync(command.CategoryId, false);

            if (category == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"You're trying to upload a image to a category that do not exist"));
                return default;
            }

            var temp = command.File.FileName.Split('.');

            var fileName = string.Join("", temp.Take(temp.Length - 1));
            var fileExtension = temp[temp.Length - 1];

            var blobName = $"{category.Id}_{fileName}_{ToUnixEpochDate(NewDateTime())}.{fileExtension}";

            var url = await _azureStorageGateway.UploadBlob("img", blobName, command.File);

            if (!string.IsNullOrEmpty(url))
            {
                category.UpdateImage(url);
            }

            category = repository.Update(category);

            return new UpdateCategoryImageCommandResponse()
            {
                Category = _mapper.Map<CategoryDTO>(category)
            };
        }

        private long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
