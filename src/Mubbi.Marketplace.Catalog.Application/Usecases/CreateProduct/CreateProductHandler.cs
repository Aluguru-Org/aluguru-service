using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Catalog.Application.ViewModels;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Application.Usecases.CreateProduct
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

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productRepository = _unitOfWork.Repository<Product>();

            var product = new Product(request.CategoryId, request.SubCategoryId, request.Name, request.Description, request.Price, request.IsActive,
                request.StockQuantity, request.RentType, request.MinRentTime, request.MaxRentTime, request.ImageUrls, request.CustomFields);

            product = await productRepository.AddAsync(product);

            return new CreateProductCommandResponse()
            {
                Product = _mapper.Map<ProductViewModel>(product)
            };
        }
    }
}
