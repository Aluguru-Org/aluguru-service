using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Application.Usecases.GetProducts
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

        public async Task<GetProductsCommandResponse> Handle(GetProductsCommand request, CancellationToken cancellationToken)
        {
            var productQueryRepository = _unitOfWork.QueryRepository<Product>();

            var paginatedProducts = await productQueryRepository.QueryAsync(
                request.PaginateCriteria, 
                product => product, 
                product => product.Include(x => x.CustomFields));

            return new GetProductsCommandResponse() { PaginatedProducts = paginatedProducts };
        }
    }
}
