using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Mubbi.Marketplace.Catalog.Events
{
    public class ProductUpdatedEvent : Event
    {
        public ProductUpdatedEvent(Guid productId, Product product)
        {
            ProductId = productId;
            Product = product;
        }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
