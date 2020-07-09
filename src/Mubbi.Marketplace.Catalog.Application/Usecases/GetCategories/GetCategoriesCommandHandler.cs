using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Catalog.Application.ViewModels;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Catalog.Repositories;
using Mubbi.Marketplace.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Application.Usecases.GetCategories
{
    public class GetCategoriesCommandHandler : IRequestHandler<GetCategoriesCommand, GetCategoriesCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoriesCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetCategoriesCommandResponse> Handle(GetCategoriesCommand request, CancellationToken cancellationToken)
        {
            var categoryQueryRepository = _unitOfWork.QueryRepository<Category>();

            var categories = await categoryQueryRepository.GetCategories();

            return new GetCategoriesCommandResponse()
            {
                Categories = _mapper.Map<List<CategoryViewModel>>(categories)
            };
        }
    }
}