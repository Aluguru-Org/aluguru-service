using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Rent.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;
using Aluguru.Marketplace.Rent.Dtos;
using AutoMapper;

namespace Aluguru.Marketplace.Rent.Usecases.GetOrders
{
    public class GetOrdersHandler : IRequestHandler<GetOrdersCommand, GetOrdersCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrdersHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GetOrdersCommandResponse> Handle(GetOrdersCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Order>();

            var paginatedProducts = await queryRepository.FindAllAsync<Order, OrderDTO>(
                _mapper,
                request.PaginateCriteria,
                order => order,
                order => (request.UserId.HasValue ? order.UserId == request.UserId.Value : true),
                null);

            return new GetOrdersCommandResponse() { PaginatedOrders = paginatedProducts };
        }
    }
}
