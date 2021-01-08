using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using Aluguru.Marketplace.Rent.Domain;
using Aluguru.Marketplace.Rent.Dtos;
using AutoMapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Usecases.GetOrdersFromCompany
{
    public class GetOrdersFromCompanyHandler : IRequestHandler<GetOrdersFromCompanyCommand, GetOrdersFromCompanyCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrdersFromCompanyHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetOrdersFromCompanyCommandResponse> Handle(GetOrdersFromCompanyCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Order>();

            var paginatedProducts = await queryRepository.FindAllAsync<Order, OrderDTO>(
                _mapper,
                request.PaginateCriteria,
                order => order,
                order => order.OrderItems.Any(x => x.CompanyId == request.CompanyId),
                null);

            for (int i = 0; i < paginatedProducts.Items.Count; i ++)
            {
                paginatedProducts.Items[i].OrderItems = 
                    paginatedProducts.Items[i].OrderItems.Where(x => x.CompanyId == request.CompanyId).ToList();
            }

            return new GetOrdersFromCompanyCommandResponse() { PaginatedOrders = paginatedProducts };
        }
    }
}
