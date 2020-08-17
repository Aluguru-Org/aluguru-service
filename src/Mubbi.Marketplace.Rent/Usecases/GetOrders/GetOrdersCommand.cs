using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Rent.Domain;
using System;

namespace Mubbi.Marketplace.Rent.Usecases.GetOrders
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
