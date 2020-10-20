using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Rent.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Usecases.GetOrders
{
    public class GetOrdersHandler : IRequestHandler<GetOrdersCommand, GetOrdersCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetOrdersHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetOrdersCommandResponse> Handle(GetOrdersCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Order>();

            var paginatedProducts = await queryRepository.FindAllAsync(
                request.PaginateCriteria,
                order => order,
                order => (request.UserId.HasValue ? order.UserId == request.UserId.Value : true),
                null);

            return new GetOrdersCommandResponse() { PaginatedOrders = paginatedProducts };
        }
    }
}
