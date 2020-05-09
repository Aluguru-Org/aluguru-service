﻿using AutoMapper;
using Mubbi.Marketplace.Catalog.Application.ViewModels;
using Mubbi.Marketplace.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Catalog.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProductViewModel, Product>()
                .ConstructUsing(p =>
                    new Product(p.CategoryId, p.Name, p.Descrpition, p.Image, p.Price, p.IsActive,
                    p.StockQuantity, p.RentType, p.MinRentTime, p.MaxRentTime, new Dimensions(p.Height, p.Width, p.Depth)));

            CreateMap<CategoryViewModel, Category>()
                .ConstructUsing(c => new Category(c.Name, c.Code));
        }
    }
}
