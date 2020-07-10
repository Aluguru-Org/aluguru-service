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

namespace Mubbi.Marketplace.Catalog.Usecases.CreateCategory
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

        public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Category>();
            var category = await queryRepository.GetCategoryByCode(request.Code);

            if (category != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"The Category {category} is already registered"));
                return new CreateCategoryCommandResponse();
            }

            var repository = _unitOfWork.Repository<Category>();

            category = new Category(request.Name, request.Code, request.MainCategoryId);

            category = await repository.AddAsync(category);

            return new CreateCategoryCommandResponse
            {
                Category = _mapper.Map<CategoryViewModel>(category)
            };
        }
    }
}
