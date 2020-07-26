using AutoMapper;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Catalog.Usecases.CreateCategory;
using Mubbi.Marketplace.Catalog.Usecases.CreateProduct;
using Mubbi.Marketplace.Catalog.Usecases.UpdateCategory;
using Mubbi.Marketplace.Catalog.ViewModels;
using System;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Catalog.AutoMapper
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
                .ConstructUsing(x => new CreateCategoryCommand(x.Name, x.MainCategoryId))
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());

            CreateMap<CategoryViewModel, Category>()
                .ConstructUsing(c => new Category(c.Name, c.MainCategoryId));

            CreateMap<ProductViewModel, Product>()
                .ConstructUsing((x, rc) =>
                {
                    var customFields = rc.Mapper.Map<List<CustomField>>(x.CustomFields);
                    var rentType = (ERentType)Enum.Parse(typeof(ERentType), x.RentType);

                    return new Product(
                        x.UserId,
                        x.CategoryId,
                        x.SubCategoryId,
                        x.Name,
                        x.Description,
                        rentType,
                        x.Price,
                        x.IsActive,
                        x.StockQuantity,
                        x.MinRentDays,
                        x.MaxRentDays,
                        x.MinNoticeRentDays,
                        x.ImageUrls,
                        customFields);
                });

            CreateMap<CreateProductViewModel, CreateProductCommand>()
                .ConstructUsing((x, rc) =>
                {
                    var customFields = rc.Mapper.Map<List<CustomField>>(x.CustomFields);
                    var rentType = (ERentType)Enum.Parse(typeof(ERentType), x.RentType);

                    return new CreateProductCommand(
                        x.UserId,
                        x.CategoryId,
                        x.SubCategoryId,
                        x.Name,
                        x.Description,
                        rentType,
                        x.Price,
                        x.IsActive,
                        x.StockQuantity,
                        x.MinRentDays,
                        x.MaxRentDays,
                        x.MinNoticeRentDays,
                        x.ImageUrls,
                        customFields);
                })
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());

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
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<CustomField, CustomFieldViewModel>();
        }
    }
}
