using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Domain;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Usecases.GetCategory
{
    public class GetCategoryHandler : IRequestHandler<GetCategoryCommand, GetCategoryCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetCategoryCommandResponse> Handle(GetCategoryCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Category>();
            var category = await queryRepository.GetCategoryAsync(request.CategoryUri);

            return new GetCategoryCommandResponse()
            {
                Category = _mapper.Map<CategoryDTO>(category)
        };
        }
    }
}
