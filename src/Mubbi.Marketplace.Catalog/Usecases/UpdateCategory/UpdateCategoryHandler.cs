using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Catalog.Data.Repositories;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Usecases.UpdateCategory
{

    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, UpdateCategoryCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public UpdateCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            if (command.CategoryId != command.Category.Id)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The provided Category Id [{command.CategoryId}] does not match with the Category passed to be updated [{command.Category.Id}]"));
                return new UpdateCategoryCommandResponse();
            }

            var queryRepository = _unitOfWork.QueryRepository<Category>();

            var existed = await queryRepository.GetCategoryAsync(command.CategoryId, false);

            if (existed == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The Category {existed} was not found"));
                return new UpdateCategoryCommandResponse();
            }

            var repository = _unitOfWork.Repository<Category>();

            var updated = existed.UpdateCategory(command);
            var category = repository.Update(updated);

            return new UpdateCategoryCommandResponse()
            {
                Category = _mapper.Map<CategoryViewModel>(category)
            };
        }
    }
}
