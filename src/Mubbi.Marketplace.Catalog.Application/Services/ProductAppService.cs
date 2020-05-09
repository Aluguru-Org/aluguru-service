using AutoMapper;
using Mubbi.Marketplace.Catalog.Application.ViewModels;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Shared.DomainObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Application.Services
{
    public class ProductAppService : IProductAppService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;
        public ProductAppService(IProductRepository productRepository,
                                 IStockService stockService,
                                 IMapper mapper)
        {
            _productRepository = productRepository;
            _stockService = stockService;
            _mapper = mapper;
        }

        public async Task<ProductViewModel> GetById(Guid id)
        {
            return _mapper.Map<ProductViewModel>(await _productRepository.GetById(id));
        }

        public async Task<IEnumerable<ProductViewModel>> GetByCategory(int code)
        {
            return _mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetByCategory(code));
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProducts()
        {
            return _mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetAllProducts());
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategories()
        {
            return _mapper.Map<IEnumerable<CategoryViewModel>>(await _productRepository.GetAllCategories());
        }

        public async Task AddProduct(ProductViewModel productViewModel)
        {
            var product = _mapper.Map<Product>(productViewModel);

            _productRepository.AddProduct(product);

            await _productRepository.UnitOfWork.Commit();
        }

        public async Task UpdateProduct(ProductViewModel productViewModel)
        {
            var product = _mapper.Map<Product>(productViewModel);

            _productRepository.UpdateProduct(product);

            await _productRepository.UnitOfWork.Commit();
        }

        public async Task<ProductViewModel> DebitStock(Guid id, int amount)
        {
            if (!await _stockService.DebitStock(id, amount))
            {
                throw new DomainException($"Failed to debit amount of {amount} from product stock");
            }

            return _mapper.Map<ProductViewModel>(await _productRepository.GetById(id));
        }

        public async Task<ProductViewModel> ReplenishStock(Guid id, int amount)
        {
            if (!await _stockService.ReplenishStock(id, amount))
            {
                throw new DomainException($"Failed to replenish amount of {amount} from product stock");
            }

            return _mapper.Map<ProductViewModel>(await _productRepository.GetById(id)); 
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
            _stockService?.Dispose();
        }
    }
}
