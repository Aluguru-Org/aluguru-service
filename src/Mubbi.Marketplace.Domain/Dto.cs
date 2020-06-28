using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Domain
{
    public interface IDto
    {
    }

    public class PaginateCriteria : IDto
    {
        private const int MaxPageSize = 50;
        private const int ConfigurablePageSize = 10;
        private const string DefaultSortBy = "Id";
        private const string DefaultSortOrder = "desc";

        private int _pageSize = MaxPageSize;
        private string _sortBy = DefaultSortBy;
        private string _sortOrder = DefaultSortOrder;

        public PaginateCriteria()
        {
            CurrentPage = 1;
            PageSize = ConfigurablePageSize;
        }

        public int CurrentPage { get; set; }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? 1 : value;
        }

        public string SortBy
        {
            get => _sortBy;
            set => _sortBy = string.IsNullOrEmpty(value) ? DefaultSortBy : value;
        }

        public string SortOrder
        {
            get => _sortOrder;
            set => _sortOrder = string.IsNullOrEmpty(value) ? DefaultSortOrder : value;
        }

        public PaginateCriteria SetPageSize(int pageSize)
        {
            Ensure.That<ValidationException>(pageSize >= 0, "PageSize should not be less than zero.");

            PageSize = pageSize;
            return this;
        }

        public PaginateCriteria SetCurrentPage(int currentPage)
        {
            Ensure.That<ValidationException>(currentPage >= 0, "CurrentPage should not be less than zero.");

            CurrentPage = currentPage;
            return this;
        }
    }
}