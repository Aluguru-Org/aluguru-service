﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Catalog.Usecases.GetProductsByCategory;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Usecases.GetProductsByCategory
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

            var paginatedProducts = await queryRepository.FindAllAsync(
                request.PaginateCriteria,
                product => product,
                product => product.CategoryId == request.CategoryId,
                product => product.Include(x => x.CustomFields));

            return new GetProductsByCategoryCommandResponse()
            {
                PaginatedProducts = paginatedProducts
            };
        }
    }
}
