using AutoMapper;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Catalog.Usecases.CreateCategory;
using Aluguru.Marketplace.Catalog.Usecases.CreateProduct;
using Aluguru.Marketplace.Catalog.Usecases.CreateRentPeriod;
using Aluguru.Marketplace.Catalog.Usecases.UpdateCategory;
using Aluguru.Marketplace.Catalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aluguru.Marketplace.Catalog.AutoMapper
{
    public class CatalogContextMappingConfiguration : Profile
    {
        public CatalogContextMappingConfiguration()
        {
            ViewModelToDomainConfiguration();
            DomainToViewModelConfiguration();
        }

        private void ViewModelToDomainConfiguration()
        {
            CreateMap<CreateRentPeriodViewModel, CreateRentPeriodCommand>()
                .ConstructUsing(x => new CreateRentPeriodCommand(x.Name, x.Days))
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());

            CreateMap<CreateCategoryViewModel, CreateCategoryCommand>()
                .ConstructUsing(x => new CreateCategoryCommand(x.Name, x.MainCategoryId))
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());

            CreateMap<CategoryViewModel, Category>()
                .ConstructUsing(c => new Category(c.Name, c.MainCategoryId));

            CreateMap<ProductViewModel, Product>()
                .ConstructUsing((x, rc) =>
                {
                    var price = rc.Mapper.Map<Price>(x.Price);
                    var customFields = rc.Mapper.Map<List<CustomField>>(x.CustomFields);
                    var rentType = (ERentType)Enum.Parse(typeof(ERentType), x.RentType);

                    return new Product(
                        x.UserId,
                        x.CategoryId,
                        x.SubCategoryId,
                        x.Name,
                        x.Description,
                        rentType,
                        price,
                        x.IsActive,
                        x.StockQuantity,
                        x.MinRentDays,
                        x.MaxRentDays,
                        x.MinNoticeRentDays,
                        customFields);
                });

            CreateMap<CreateProductViewModel, CreateProductCommand>()
                .ConstructUsing((x, rc) =>
                {
                    var price = rc.Mapper.Map<Price>(x.Price);
                    var customFields = rc.Mapper.Map<List<CustomField>>(x.CustomFields);
                    var rentType = (ERentType)Enum.Parse(typeof(ERentType), x.RentType);

                    return new CreateProductCommand(
                        x.UserId,
                        x.CategoryId,
                        x.SubCategoryId,
                        x.Name,
                        x.Description,
                        rentType,
                        price,
                        x.IsActive,
                        x.StockQuantity,
                        x.MinRentDays,
                        x.MaxRentDays,
                        x.MinNoticeRentDays,
                        customFields);
                })
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());

            CreateMap<PriceViewModel, Price>()
                .ConstructUsing((x, rc) =>
                {
                    var periodPrices = rc.Mapper.Map<List<PeriodPrice>>(x.PeriodRentPrices);
                    return new Price(x.SellPrice, x.DailyRentPrice, periodPrices);
                });

            CreateMap<PeriodPriceViewModel, PeriodPrice>()
                .ConstructUsing((x, rc) => new PeriodPrice(x.RentPeriodId, x.Price));

            CreateMap<CreateCustomFieldViewModel, CustomField>()
                .ConstructUsing((x, rc) =>
                {
                    var fieldType = (EFieldType)Enum.Parse(typeof(EFieldType), x.FieldType);
                    switch (fieldType)
                    {
                        default:
                        case EFieldType.Text:
                            return new CustomField(x.ValueAsString);
                        case EFieldType.Number:
                            return new CustomField(x.ValueAsInt.Value);
                        case EFieldType.Radio:
                        case EFieldType.Checkbox:
                            return new CustomField(fieldType, x.ValueAsOptions);
                    }
                })
                .ForMember(x => x.Id, c => c.Ignore())
                .ForMember(x => x.ProductId, c => c.Ignore())
                .ForMember(x => x.Product, c => c.Ignore())
                .ForMember(x => x.DateCreated, c => c.Ignore())
                .ForMember(x => x.DateUpdated, c => c.Ignore());
        }

        private void DomainToViewModelConfiguration()
        {
            CreateMap<PeriodPrice, PeriodPriceViewModel>();
            CreateMap<Price, PriceViewModel>();
            CreateMap<RentPeriod, RentPeriodViewModel>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CustomField, CustomFieldViewModel>();
            CreateMap<Product, ProductViewModel>();
        }
    }
}
