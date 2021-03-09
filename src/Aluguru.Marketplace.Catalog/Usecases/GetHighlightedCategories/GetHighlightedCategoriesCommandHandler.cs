using MediatR;
using Microsoft.EntityFrameworkCore;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Aluguru.Marketplace.Catalog.Dtos;

namespace Aluguru.Marketplace.Catalog.Usecases.GetHighlightedCategories
{
    public class GetHighlightedCategoriesCommandHandler : IRequestHandler<GetHighlightedCategoriesCommand, GetHighlightedCategoriesCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetHighlightedCategoriesCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetHighlightedCategoriesCommandResponse> Handle(GetHighlightedCategoriesCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Category>();

            var paginatedCategories = await queryRepository.FindAllAsync<Category, CategoryDTO>(
                _mapper,
                request.PaginateCriteria,
                category => category,
                category => !category.MainCategoryId.HasValue && category.Highlights,
                category => category.Include(x => x.SubCategories));

            return new GetHighlightedCategoriesCommandResponse()
            {
                Categories = paginatedCategories
            };
        }
    }
}