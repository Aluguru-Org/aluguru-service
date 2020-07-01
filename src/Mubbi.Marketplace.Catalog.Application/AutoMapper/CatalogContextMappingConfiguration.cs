using AutoMapper;
using Mubbi.Marketplace.Catalog.Application.ViewModels;
using Mubbi.Marketplace.Catalog.Domain;

namespace Mubbi.Marketplace.Catalog.Application.AutoMapper
{
    public class CatalogContextMappingConfiguration : Profile
    {
        public CatalogContextMappingConfiguration()
        {
            CreateMap<ProductViewModel, Product>()
                .ConstructUsing(p =>
                    new Product(p.CategoryId, p.Name, p.Descrpition, p.Image, p.Price, p.IsActive,
                    p.StockQuantity, p.RentType, p.MinRentTime, p.MaxRentTime));

            CreateMap<CategoryViewModel, Category>()
                .ConstructUsing(c => new Category(c.Name, c.Code));
        }
    }
}
