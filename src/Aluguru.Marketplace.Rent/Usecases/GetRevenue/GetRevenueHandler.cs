using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Rent.Domain;
using System.Threading;
using System.Threading.Tasks;
using Aluguru.Marketplace.Infrastructure.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Aluguru.Marketplace.Rent.Usecases.GetRevenue
{
    public class GetRevenueHandler : IRequestHandler<GetRevenueCommand, GetRevenueCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRevenueHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetRevenueCommandResponse> Handle(GetRevenueCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Order>();

            var orders = await queryRepository.ListAsync(
                order =>
                    (order.OrderStatus == EOrderStatus.PaymentConfirmed) &&
                    (order.DateCreated >= command.StartDate && order.DateCreated <= command.EndDate) &&
                    (command.CompanyId.HasValue ? order.OrderItems.Any(item => item.CompanyId == command.CompanyId) : true),
                order => order.Include(x => x.OrderItems));

            var revenue = 0m;

            if (command.CompanyId.HasValue)
            {
                revenue = orders
                    .Select(x => x.OrderItems.Where(y => y.CompanyId == command.CompanyId)
                                             .Sum(y => y.CalculatePrice()))
                    .Sum();
            } 
            else
            {
                revenue = orders.Sum(x => x.TotalPrice);
            }

            return new GetRevenueCommandResponse()
            {
                Revenue = revenue
            };
        }
    }
}
