using Mubbi.Marketplace.Catalog.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Application.Services
{
    public interface IProductAppService : IDisposable
    {
        Task<ProductViewModel> GetById(Guid id);
        Task<IEnumerable<ProductViewModel>> GetByCategory(int code);
        Task<IEnumerable<ProductViewModel>> GetAllProducts();
        Task<IEnumerable<CategoryViewModel>> GetAllCategories();

        Task AddProduct(ProductViewModel productViewModel);
        Task UpdateProduct(ProductViewModel productViewModel);

        Task<ProductViewModel> DebitStock(Guid id, int amount);
        Task<ProductViewModel> ReplenishStock(Guid id, int amount);
    }
}
