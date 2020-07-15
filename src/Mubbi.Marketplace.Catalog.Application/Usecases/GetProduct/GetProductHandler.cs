 using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using System.Threading;
using System.Threading.Tasks;
using Mubbi.Marketplace.Catalog.Data.Repositories;

namespace Mubbi.Marketplace.Catalog.Usecases.GetProduct
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

            var product = await queryRepository.GetProductAsync(request.ProductId);

            return new GetProductCommandResponse()
            {
                Product = _mapper.Map<ProductViewModel>(product)
            };
        }
    }
}
