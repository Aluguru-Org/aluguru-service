using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Catalog.Application.ViewModels;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Catalog.Repositories;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Data;
using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Application.Services
{
    public class ProductAppService : IProductAppService
    {
        private readonly IEfUnitOfWork _unitOfWork;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;
        public ProductAppService(IEfUnitOfWork unitOfWork,
                                 IStockService stockService,
                                 IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _stockService = stockService;
            _mapper = mapper;
        }

        public async Task<ProductViewModel> GetById(Guid id)
        {
            return _mapper.Map<ProductViewModel>(await _unitOfWork.QueryRepository<Product>().GetProductAsync(id));
        }

        public async Task<IEnumerable<ProductViewModel>> GetByCategory(int code)
        {
            return _mapper.Map<IEnumerable<ProductViewModel>>(await _unitOfWork.QueryRepository<Category>().GetCategoryByCode(code));
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProducts()
        {
            return _mapper.Map<IEnumerable<ProductViewModel>>(await _unitOfWork.QueryRepository<Product>().Queryable().ToListAsync());
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategories()
        {
            return _mapper.Map<IEnumerable<CategoryViewModel>>(await _unitOfWork.QueryRepository<Category>().Queryable().ToListAsync());
        }

        public async Task AddProduct(ProductViewModel productViewModel)
        {
            var product = _mapper.Map<Product>(productViewModel);
            _unitOfWork.Repository<Product>().Add(product);

            await _unitOfWork.SaveChangesAsync(new CancellationToken());
        }

        public async Task UpdateProduct(ProductViewModel productViewModel)
        {
            var product = _mapper.Map<Product>(productViewModel);

            _unitOfWork.Repository<Product>().Update(product);

            await _unitOfWork.SaveChangesAsync(new CancellationToken());
        }

        public async Task<ProductViewModel> DebitStock(Guid id, int amount)
        {
            Ensure.That<DomainException>(await _stockService.DebitStock(id, amount), $"Failed to debit amount of {amount} from product stock");

            return _mapper.Map<ProductViewModel>(await _unitOfWork.QueryRepository<Product>().GetProductAsync(id));
        }

        public async Task<ProductViewModel> ReplenishStock(Guid id, int amount)
        {
            if (!await _stockService.ReplenishStock(id, amount))
            {
                throw new DomainException($"Failed to replenish amount of {amount} from product stock");
            }

            return _mapper.Map<ProductViewModel>(await _unitOfWork.QueryRepository<Product>().GetProductAsync(id)); 
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
            _stockService?.Dispose();
        }
    }
}
