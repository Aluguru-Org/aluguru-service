using Mubbi.Marketplace.Shared.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Domain
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetById(Guid id);
        Task<IEnumerable<Product>> GetByCategory(int code);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Category>> GetAllCategories();

        void AddProduct(Product product);
        void UpdateProduct(Product product);

        void AddCategory(Category category);
        void UpdateCategory(Category category);
    }
}
