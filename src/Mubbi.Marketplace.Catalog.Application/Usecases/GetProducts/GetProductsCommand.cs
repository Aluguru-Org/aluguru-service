using Mubbi.Marketplace.Catalog.Application.ViewModels;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Catalog.Application.Usecases.GetProducts
{
    public class GetProductsCommand : Command<GetProductsCommandResponse>
    {
        public GetProductsCommand(PaginateCriteria paginateCriteria)
        {
            PaginateCriteria = paginateCriteria;
        }

        public PaginateCriteria PaginateCriteria { get; private set; }
    }

    public class GetProductsCommandResponse
    {
        public PaginatedItem<Product> PaginatedProducts { get; set; }
    }
}
