using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Usecases.CreateProduct
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
                command.ImageUrls, 
                command.CustomFields);

            product = await productRepository.AddAsync(product);

            return new CreateProductCommandResponse()
            {
                Product = _mapper.Map<ProductViewModel>(product)
            };
        }
    }
}
