﻿using AutoMapper;
using Aluguru.Marketplace.Rent.Domain;
using Aluguru.Marketplace.Rent.Usecases.CreateOrder;
using Aluguru.Marketplace.Rent.Dtos;
using Aluguru.Marketplace.Rent.Usecases.CreateVoucher;

namespace Aluguru.Marketplace.Rent.AutoMapper
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
            CreateMap<CreateOrderDTO, CreateOrderCommand>()
                .ConstructUsing(x => new CreateOrderCommand(x.UserId, x.OrderItems))
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());

            CreateMap<CreateVoucherDTO, CreateVoucherCommand>()
                .ConstructUsing(x => new CreateVoucherCommand(x))
                .ForMember(x => x.Voucher, c => c.Ignore())
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());
        }

        private void DomainToViewModelConfiguration()
        {
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<Voucher, VoucherDTO>();
        }
    }
}
