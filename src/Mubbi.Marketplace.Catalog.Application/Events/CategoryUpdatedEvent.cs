using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Mubbi.Marketplace.Catalog.Events
{
    public class CategoryUpdatedEvent : Event
    {
        public CategoryUpdatedEvent(Guid categoryId, string name, int code, Guid? mainCategoryId)
        {
            AggregateId = categoryId;
            CategoryId = categoryId;
            Name = name;
            Code = code;
            MainCategoryId = mainCategoryId;
        }

        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public Guid? MainCategoryId { get; set; }
    }
}
