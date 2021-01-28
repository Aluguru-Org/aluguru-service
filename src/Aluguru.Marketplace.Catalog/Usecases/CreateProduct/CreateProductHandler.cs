using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;

namespace Aluguru.Marketplace.Catalog.Usecases.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public CreateProductHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Product>();

            if (await queryRepository.GetProductAsync(command.Uri) != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The Product Uri {command.Uri} is already registered"));
                return default;
            }

            var productRepository = _unitOfWork.Repository<Product>();

            var product = new Product(
                command.UserId, 
                command.CategoryId, 
                command.SubCategoryId, 
                command.Name, 
                command.Uri,
                command.Description, 
                command.RentType,
                command.Price, 
                command.IsActive,
                command.StockQuantity, 
                command.MinRentDays, 
                command.MaxRentDays, 
                command.MinNoticeRentDays,
                command.InvalidDates,
                command.CustomFields);

            product = await productRepository.AddAsync(product);

            return new CreateProductCommandResponse()
            {
                Product = _mapper.Map<ProductDTO>(product)
            };
        }
    }
}
