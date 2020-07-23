using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Mubbi.Marketplace.Catalog.Usecases.GetProducts
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
