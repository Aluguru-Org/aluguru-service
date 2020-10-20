using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Catalog.ViewModels;
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
            var category = await queryRepository.GetCategoryByNameAsync(command.Name);

            if (category != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The Category {category} is already registered"));
                return new CreateCategoryCommandResponse();
            }

            var repository = _unitOfWork.Repository<Category>();

            category = new Category(command.Name, command.MainCategoryId);

            category = await repository.AddAsync(category);

            return new CreateCategoryCommandResponse
            {
                Category = _mapper.Map<CategoryViewModel>(category)
            };
        }
    }
}
