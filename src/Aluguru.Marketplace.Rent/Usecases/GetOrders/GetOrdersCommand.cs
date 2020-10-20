using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.Domain;
using System;

namespace Aluguru.Marketplace.Rent.Usecases.GetOrders
{
    public class GetOrdersCommand : Command<GetOrdersCommandResponse>
    {
        public GetOrdersCommand(Guid? userId, PaginateCriteria paginateCriteria)
        {
            UserId = userId;
            PaginateCriteria = paginateCriteria;
        }

        public Guid? UserId { get; private set; }
        public PaginateCriteria PaginateCriteria { get; private set; }
    }

    public class GetOrdersCommandResponse
    {
        public PaginatedItem<Order> PaginatedOrders { get; set; }
    }
}
