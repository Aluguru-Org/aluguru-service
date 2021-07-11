using MediatR;
using Microsoft.EntityFrameworkCore;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Aluguru.Marketplace.Catalog.Dtos;

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
            var queryRepository = _unitOfWork.QueryRepository<Category>();

            

            var paginatedCategories = await queryRepository.FindAllAsync<Category, CategoryDTO>(
                _mapper,
                request.PaginateCriteria,
                category => category,
                category => !category.MainCategoryId.HasValue,
                category => category.Include(x => x.SubCategories));

            return new GetCategoriesCommandResponse()
            {
                Categories = paginatedCategories
            };
        }
    }
}