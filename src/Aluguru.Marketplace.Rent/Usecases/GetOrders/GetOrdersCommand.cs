using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.Dtos;
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
        public PaginatedItem<OrderDTO> PaginatedOrders { get; set; }
    }
}
