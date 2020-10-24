using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Catalog.ViewModels;
using Aluguru.Marketplace.Crosscutting.AzureStorage;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Usecases.DeleteCategoryImage
{
    public class DeleteCategoryImageHandler : IRequestHandler<DeleteCategoryImageCommand, DeleteCategoryImageCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IAzureStorageGateway _azureStorageGateway;
        public DeleteCategoryImageHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler, IAzureStorageGateway azureStorageGateway)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
            _azureStorageGateway = azureStorageGateway;
        }

        public async Task<DeleteCategoryImageCommandResponse> Handle(DeleteCategoryImageCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Category>();
            var repository = _unitOfWork.Repository<Category>();

            var category = await queryRepository.GetCategoryAsync(command.CategoryId, false);

            if (category == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"You're trying to delete a image from a product that do not exist"));
                return default;
            }

            var array = category.ImageUrl.Split('/');
            var fileName = array[array.Length - 1];

            await _azureStorageGateway.DeleteBlob("img", fileName);
            category.RemoveImage();

            category = repository.Update(category);

            return new DeleteCategoryImageCommandResponse()
            {
                Category = _mapper.Map<CategoryViewModel>(category)
            };
        }
    }
}
