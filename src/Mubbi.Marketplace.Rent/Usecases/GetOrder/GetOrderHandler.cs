using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Rent.Data.Repositories;
using Mubbi.Marketplace.Rent.Domain;
using Mubbi.Marketplace.Rent.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Rent.Usecases.GetOrder
{
    public class GetOrderHandler : IRequestHandler<GetOrderCommand, GetOrderCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrderHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetOrderCommandResponse> Handle(GetOrderCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Order>();

            var order = await queryRepository.GetOrderAsync(request.OrderId);

            return new GetOrderCommandResponse()
            {
                Order = _mapper.Map<OrderViewModel>(order)
            };
        }
    }
}
