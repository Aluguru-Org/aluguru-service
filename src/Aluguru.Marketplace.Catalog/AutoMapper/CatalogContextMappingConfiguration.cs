using AutoMapper;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Catalog.Usecases.CreateCategory;
using Aluguru.Marketplace.Catalog.Usecases.CreateProduct;
using Aluguru.Marketplace.Catalog.Usecases.CreateRentPeriod;
using Aluguru.Marketplace.Catalog.Dtos;
using System;
using System.Collections.Generic;

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
            CreateMap<CreateRentPeriodDTO, CreateRentPeriodCommand>()
                .ConstructUsing(x => new CreateRentPeriodCommand(x.Name, x.Days))
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());

            CreateMap<CreateCategoryDTO, CreateCategoryCommand>()
                .ConstructUsing(x => new CreateCategoryCommand(x.Name, x.Uri, x.MainCategoryId))
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());

            CreateMap<CategoryDTO, Category>()
                .ConstructUsing(c => new Category(c.Name, c.Uri, c.MainCategoryId));

            CreateMap<ProductDTO, Product>()
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
                        x.Uri,
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
                .ForMember(x => x.Price, c => c.Ignore())
                .ForMember(x => x.CustomFields, c => c.Ignore());
    
            CreateMap<CreateProductDTO, CreateProductCommand>()
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
                        x.Uri,
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
                .ForMember(x => x.ValidationResult, c => c.Ignore())
                .ForMember(x => x.Price, c => c.Ignore())
                .ForMember(x => x.CustomFields, c => c.Ignore());

            CreateMap<PriceDTO, Price>()
                .ConstructUsing((x, rc) =>
                {
                    var periodPrices = rc.Mapper.Map<List<PeriodPrice>>(x.PeriodRentPrices);
                    return new Price(x.FreightPriceKM, x.SellPrice, x.DailyRentPrice, periodPrices);
                });

            CreateMap<PeriodPriceViewModel, PeriodPrice>()
                .ConstructUsing((x, rc) => new PeriodPrice(x.RentPeriodId, x.Price));

            CreateMap<CustomFieldDTO, CustomField>()
                .ConstructUsing((x, rc) =>
                {
                    var fieldType = (EFieldType)Enum.Parse(typeof(EFieldType), x.FieldType);
                    switch (fieldType)
                    {
                        default:
                        case EFieldType.Text:
                            return new CustomField(EFieldType.Text, x.FieldName);
                        case EFieldType.Number:
                            return new CustomField(EFieldType.Number, x.FieldName);
                        case EFieldType.Radio:
                        case EFieldType.Checkbox:
                            return new CustomField(fieldType, x.FieldName, x.ValueAsOptions);
                    }
                })
                .ForMember(x => x.Id, c => c.Ignore())
                .ForMember(x => x.ProductId, c => c.Ignore())
                .ForMember(x => x.Product, c => c.Ignore())
                .ForMember(x => x.DateCreated, c => c.Ignore())
                .ForMember(x => x.DateUpdated, c => c.Ignore());

            CreateMap<CreateCustomFieldDTO, CustomField>()
                .ConstructUsing((x, rc) =>
                {
                    var fieldType = (EFieldType)Enum.Parse(typeof(EFieldType), x.FieldType);
                    switch (fieldType)
                    {
                        default:
                        case EFieldType.Text:
                            return new CustomField(EFieldType.Text, x.FieldName);
                        case EFieldType.Number:
                            return new CustomField(EFieldType.Number, x.FieldName);
                        case EFieldType.Radio:
                        case EFieldType.Checkbox:
                            return new CustomField(fieldType, x.FieldName, x.ValueAsOptions);
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
            CreateMap<Price, PriceDTO>();
            CreateMap<RentPeriod, RentPeriodDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CustomField, CustomFieldDTO>();
            CreateMap<Product, ProductDTO>();
        }
    }
}
