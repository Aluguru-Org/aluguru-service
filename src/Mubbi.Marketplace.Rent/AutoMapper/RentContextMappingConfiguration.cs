using AutoMapper;
using Mubbi.Marketplace.Rent.Domain;
using Mubbi.Marketplace.Rent.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Rent.AutoMapper
{
    public class RentContextMappingConfiguration : Profile
    {
        public RentContextMappingConfiguration()
        {
            DomainToViewModelConfiguration();
        }

        private void DomainToViewModelConfiguration()
        {
            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderItem, OrderItemViewModel>();
            CreateMap<Voucher, VoucherViewModel>();
        }
    }
}
