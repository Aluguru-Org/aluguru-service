using MediatR;
using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Application.Usecases.GetProducts
{
    public class GetProductsHandler : IRequestHandler<GetProductsCommand, GetProductsCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetProductsCommandResponse> Handle(GetProductsCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Product>();

            var paginatedProducts = await queryRepository.QueryAsync(
                request.PaginateCriteria, 
                product => product, 
                product => product.Include(x => x.CustomFields));

            return new GetProductsCommandResponse() { PaginatedProducts = paginatedProducts };
        }
    }
}
