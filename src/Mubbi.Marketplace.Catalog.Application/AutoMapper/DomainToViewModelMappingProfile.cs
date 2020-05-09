using AutoMapper;
using Mubbi.Marketplace.Catalog.Application.ViewModels;
using Mubbi.Marketplace.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Catalog.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(d => d.Height, o => o.MapFrom(p => p.Dimensions.Height))
                .ForMember(d => d.Width, o => o.MapFrom(p => p.Dimensions.Width))
                .ForMember(d => d.Depth, o => o.MapFrom(p => p.Dimensions.Depth));

            CreateMap<Category, CategoryViewModel>();
        }
    }
}
