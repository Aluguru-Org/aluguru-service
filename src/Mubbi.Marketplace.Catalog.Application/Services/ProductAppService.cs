using Mubbi.Marketplace.Catalog.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Application.Services
{
    public class ProductAppService : IProductAppService
    {
        public Task AddProduct(ProductViewModel productViewModel)
        {            
            throw new NotImplementedException();
        }

        public Task<ProductViewModel> DebitStock(Guid id, int amount)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CategoryViewModel>> GetAllCategories()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductViewModel>> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductViewModel>> GetByCategory(int code)
        {
            throw new NotImplementedException();
        }

        public Task<ProductViewModel> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductViewModel> ReplenishStock(Guid id, int amount)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProduct(ProductViewModel productViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
