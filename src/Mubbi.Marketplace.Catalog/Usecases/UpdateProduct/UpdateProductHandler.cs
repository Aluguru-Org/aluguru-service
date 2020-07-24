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

namespace Mubbi.Marketplace.Catalog.Usecases.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public UpdateProductHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Product>();

            var existed = await queryRepository.GetProductAsync(command.ProductId);

            if (existed == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The Category {existed} was not found"));
                return new UpdateProductCommandResponse();
            }

            var repository = _unitOfWork.Repository<Product>();

            var updated = existed.UpdateProduct(command);
            var category = repository.Update(updated);

            return new UpdateProductCommandResponse()
            {
                Product = _mapper.Map<ProductViewModel>(category)
            };
        }
    }
}
