using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.Dtos;
using System;

namespace Aluguru.Marketplace.Rent.Usecases.GetOrdersFromCompany
{
    public class GetOrdersFromCompanyCommand : Command<GetOrdersFromCompanyCommandResponse>
    {
        public GetOrdersFromCompanyCommand(Guid companyId, PaginateCriteria paginateCriteria)
        {
            CompanyId = companyId;
            PaginateCriteria = paginateCriteria;
        }

        public Guid CompanyId { get; private set; }
        public PaginateCriteria PaginateCriteria { get; private set; }
    }

    public class GetOrdersFromCompanyCommandResponse
    {
        public PaginatedItem<OrderDTO> PaginatedOrders { get; set; }
    }
}
