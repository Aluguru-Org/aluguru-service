using AutoMapper;
using Mubbi.Marketplace.Register.Usecases.CreateUser;
using Mubbi.Marketplace.Register.Usecases.LogInUser;
using Mubbi.Marketplace.Register.ViewModels;
using Mubbi.Marketplace.Register.Domain;
using System;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Register.AutoMapper
{
    public class RegisterContextMappingConfiguration : Profile
    {
        public RegisterContextMappingConfiguration()
        {
            DomainToViewModelConfiguration();
            ViewModelToDomainConfiguration();
        }

        private void DomainToViewModelConfiguration()
        {
            CreateMap<UserClaim, UserClaimViewModel>();
            CreateMap<UserRole, UserRoleViewModel>();

            CreateMap<Address, AddressViewModel>();
            CreateMap<Document, DocumentViewModel>()
                .ForMember(x => x.Type, opt => opt.MapFrom(c => c.DocumentType.ToString()));
            CreateMap<User, UserViewModel>()
                .ConstructUsing((x, rc) =>
                {
                    var document = rc.Mapper.Map<DocumentViewModel>(x.Document);
                    var addresses = rc.Mapper.Map<List<AddressViewModel>>(x.Addresses);

                    return new UserViewModel()
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        Email = x.Email,
                        Addresses = addresses,
                        Document = document
                    };
                });
        }

        private void ViewModelToDomainConfiguration()
        {
            CreateMap<UserLoginViewModel, LogInUserCommand>()
                .ConstructUsing(x => new LogInUserCommand(x.UserName, x.Password))
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());

            CreateMap<UserRegistrationViewModel, CreateUserCommand>()
                .ConstructUsing((x, rc) =>
                {
                    return new CreateUserCommand(
                        x.FullName,
                        x.Password,
                        x.Email,
                        x.Role);
                })
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());
        }
    }
}