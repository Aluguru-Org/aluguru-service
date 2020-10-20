using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Catalog.Usecases.GetProducts
{
    public class GetProductsCommand : Command<GetProductsCommandResponse>
    {
        public GetProductsCommand(Guid? userId, PaginateCriteria paginateCriteria)
        {
            UserId = userId;
            PaginateCriteria = paginateCriteria;
        }

        public Guid? UserId { get; private set; }
        public PaginateCriteria PaginateCriteria { get; private set; }
    }

    public class GetProductsCommandResponse
    {
        public PaginatedItem<Product> PaginatedProducts { get; set; }
    }
}
