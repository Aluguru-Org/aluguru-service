using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mubbi.Marketplace.Catalog.Data.Repositories;

namespace Mubbi.Marketplace.Catalog.Usecases.GetCategories
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

            var categories = await categoryQueryRepository.GetCategoriesAsync();

            return new GetCategoriesCommandResponse()
            {
                Categories = _mapper.Map<List<CategoryViewModel>>(categories)
            };
        }
    }
}