using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Catalog.ViewModels;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aluguru.Marketplace.Catalog.Data.Repositories;

namespace Aluguru.Marketplace.Catalog.Usecases.GetCategories
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