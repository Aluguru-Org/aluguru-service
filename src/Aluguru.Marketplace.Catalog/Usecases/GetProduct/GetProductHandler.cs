 using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Catalog.ViewModels;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using System.Threading;
using System.Threading.Tasks;
using Aluguru.Marketplace.Catalog.Data.Repositories;

namespace Aluguru.Marketplace.Catalog.Usecases.GetProduct
{
    public class GetProductHandler : IRequestHandler<GetProductCommand, GetProductCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetProductCommandResponse> Handle(GetProductCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Product>();

            var product = await queryRepository.GetProductAsync(request.ProductUri);

            return new GetProductCommandResponse()
            {
                Product = _mapper.Map<ProductViewModel>(product)
            };
        }
    }
}
