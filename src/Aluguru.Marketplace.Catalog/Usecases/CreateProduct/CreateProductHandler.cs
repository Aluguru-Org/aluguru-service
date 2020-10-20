using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Catalog.ViewModels;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Usecases.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateProductHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var productRepository = _unitOfWork.Repository<Product>();

            var product = new Product(
                command.UserId, 
                command.CategoryId, 
                command.SubCategoryId, 
                command.Name, 
                command.Description, 
                command.RentType,
                command.Price, 
                command.IsActive,
                command.StockQuantity, 
                command.MinRentDays, 
                command.MaxRentDays, 
                command.MinNoticeRentDays,
                command.CustomFields);

            product = await productRepository.AddAsync(product);

            return new CreateProductCommandResponse()
            {
                Product = _mapper.Map<ProductViewModel>(product)
            };
        }
    }
}
