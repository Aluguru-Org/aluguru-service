using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Usecases.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public CreateCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Category>();

            if (await queryRepository.GetCategoryByNameAsync(command.Name) != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The Category {command.Name} is already registered"));
                return default;
            }

            if (await queryRepository.GetCategoryByUriAsync(command.Uri) != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The Category Uri {command.Uri} is already registered"));
                return default;
            }

            var repository = _unitOfWork.Repository<Category>();

            var category = new Category(command.Name, command.Uri, command.MainCategoryId);

            category = await repository.AddAsync(category);

            return new CreateCategoryCommandResponse
            {
                Category = _mapper.Map<CategoryDTO>(category)
            };
        }
    }
}
