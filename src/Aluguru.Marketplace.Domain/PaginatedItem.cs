﻿using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Domain
{
    public class PaginatedItem<TResponse> : ValueObject where TResponse : IDto
    {
        public PaginatedItem(long totalItems, long totalPages, IReadOnlyList<TResponse> items)
        {
            TotalItems = totalItems;
            TotalPages = totalPages;
            Items = items;
        }

        public long TotalItems { get; }
        public long TotalPages { get; }
        public IReadOnlyList<TResponse> Items { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TotalItems;
            yield return TotalPages;
            yield return Items;
        }

        protected override void ValidateValueObject()
        {
            Ensure.That(TotalItems >= 0);
            Ensure.That(TotalPages >= 0);
        }
    }
}
