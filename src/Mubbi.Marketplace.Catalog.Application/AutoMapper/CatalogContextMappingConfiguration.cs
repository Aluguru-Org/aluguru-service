using AutoMapper;
using Mubbi.Marketplace.Catalog.Application.Usecases.CreateCategory;
using Mubbi.Marketplace.Catalog.Application.Usecases.CreateProduct;
using Mubbi.Marketplace.Catalog.Application.ViewModels;
using Mubbi.Marketplace.Catalog.Domain;
using System;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Catalog.Application.AutoMapper
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
            CreateMap<CreateCategoryViewModel, CreateCategoryCommand>()
                .ConstructUsing(x => new CreateCategoryCommand(x.Name, x.Code))
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.AggregateId, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());

            CreateMap<CategoryViewModel, Category>()
                .ConstructUsing(c => new Category(c.Name, c.Code));

            CreateMap<ProductViewModel, Product>()
                .ConstructUsing((x, rc) =>
                {
                    var customFields = rc.Mapper.Map<List<CustomField>>(x.CustomFields);

                    return new Product(
                        x.CategoryId,
                        x.SubCategoryId,
                        x.Name,
                        x.Description,
                        x.Price,
                        x.IsActive,
                        x.StockQuantity,
                        x.RentType,
                        x.MinRentTime,
                        x.MaxRentTime,
                        x.ImageUrls,
                        customFields);
                });

            CreateMap<CreateProductViewModel, CreateProductCommand>()
                .ConstructUsing((x, rc) =>
                {
                    var customFields = rc.Mapper.Map<List<CustomField>>(x.CustomFields);

                    return new CreateProductCommand(
                        x.CategoryId,
                        x.SubCategoryId,
                        x.Name,
                        x.Description,
                        x.Price,
                        x.IsActive,
                        x.StockQuantity,
                        (ERentType)Enum.Parse(typeof(ERentType), x.RentType),
                        x.MinRentTime,
                        x.MaxRentTime,
                        x.ImageUrls,
                        customFields);
                })
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.AggregateId, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());

            CreateMap<CustomFieldViewModel, CustomField>()
                .ConstructUsing((x, rc) =>
                {
                    var fieldType = (EFieldType)Enum.Parse(typeof(EFieldType), x.FieldType);
                    switch(fieldType)
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
                });
        }

        private void DomainToViewModelConfiguration()
        {
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<CustomField, CustomFieldViewModel>();
        }
    }
}
