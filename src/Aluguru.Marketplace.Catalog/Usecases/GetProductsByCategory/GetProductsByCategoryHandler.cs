using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;
using Aluguru.Marketplace.Catalog.Dtos;

namespace Aluguru.Marketplace.Catalog.Usecases.GetProductsByCategory
{
    public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryCommand, GetProductsByCategoryCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsByCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetProductsByCategoryCommandResponse> Handle(GetProductsByCategoryCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Product>();

            var paginatedProducts = await queryRepository.FindAllAsync<Product, ProductDTO>(
                _mapper,
                request.PaginateCriteria,
                product => product,
                product => product.Category.Uri.Trim().ToLower() == request.Category.Trim().ToLower(),
                product => product.Include(x => x.CustomFields));

            return new GetProductsByCategoryCommandResponse()
            {
                PaginatedProducts = paginatedProducts
            };
        }
    }
}
