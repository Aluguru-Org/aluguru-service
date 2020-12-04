using MediatR;
using Microsoft.EntityFrameworkCore;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Aluguru.Marketplace.Catalog.Dtos;

namespace Aluguru.Marketplace.Catalog.Usecases.GetProducts
{
    public class GetProductsHandler : IRequestHandler<GetProductsCommand, GetProductsCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetProductsCommandResponse> Handle(GetProductsCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Product>();

            var paginatedProducts = await queryRepository.FindAllAsync<Product, ProductDTO>(
                _mapper,
                command.PaginateCriteria,
                product => product,
                product => (command.UserId.HasValue ? product.UserId == command.UserId.Value : true),
                product => product.Include(x => x.CustomFields));

            return new GetProductsCommandResponse() { PaginatedProducts = paginatedProducts };
        }
    }
}