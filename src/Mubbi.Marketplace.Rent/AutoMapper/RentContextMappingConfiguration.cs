using AutoMapper;
using Mubbi.Marketplace.Rent.Domain;
using Mubbi.Marketplace.Rent.Usecases.CreateOrder;
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
            ViewModelToDomainConfiguration();
            DomainToViewModelConfiguration();
        }

        private void ViewModelToDomainConfiguration()
        {
            CreateMap<CreateOrderViewModel, CreateOrderCommand>()
                .ConstructUsing(x => new CreateOrderCommand(x.UserId, x.OrderItems))
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());
        }

        private void DomainToViewModelConfiguration()
        {
            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderItem, OrderItemViewModel>();
            CreateMap<Voucher, VoucherViewModel>();
        }
    }
}
