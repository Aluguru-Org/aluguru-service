using AutoMapper;
using Mubbi.Marketplace.Register.Application.Usecases.CreateUser;
using Mubbi.Marketplace.Register.Application.Usecases.LogInUser;
using Mubbi.Marketplace.Register.Application.ViewModels;
using Mubbi.Marketplace.Register.Domain;
using Mubbi.Marketplace.Register.Domain.Models;
using System;

namespace Mubbi.Marketplace.Register.Application.AutoMapper
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
            CreateMap<Address, AddressViewModel>();
            CreateMap<Document, DocumentViewModel>()
                .ForMember(x => x.Type, opt => opt.MapFrom(c => c.DocumentType.ToString()));
            CreateMap<User, UserViewModel>()
                .ConstructUsing((x, rc) =>
                {
                    var address = rc.Mapper.Map<AddressViewModel>(x.Address);
                    var document = rc.Mapper.Map<DocumentViewModel>(x.Document);

                    return new UserViewModel()
                    {
                        Id = x.Id,
                        Role = x.Role.ToString(),
                        FullName = x.FullName,
                        Email = x.Email.Address,
                        Address = address,
                        Document = document
                    };
                });
        }

        private void ViewModelToDomainConfiguration()
        {
            CreateMap<AddressViewModel, Address>()
                .ConstructUsing(x => new Address(
                    x.Street,
                    x.Number,
                    x.Neighborhood,
                    x.City,
                    x.State,
                    x.Country,
                    x.ZipCode));

            CreateMap<DocumentViewModel, Document>()
                .ConstructUsing(x => new Document(x.Number, (EDocumentType)Enum.Parse(typeof(EDocumentType), x.Type)))
                .ForMember(x => x.DocumentType, c => c.Ignore());

            CreateMap<UserLoginViewModel, LogInUserCommand>()
                .ConstructUsing(x => new LogInUserCommand(x.UserName, x.Password))
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.AggregateId, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());

            CreateMap<UserRegistrationViewModel, CreateUserCommand>()
                .ConstructUsing((x, rc) =>
                {
                    return new CreateUserCommand(
                        x.FullName,
                        x.Password,
                        x.Email,
                        x.Role,
                        x.Document.Type,
                        x.Document.Number,
                        x.Address.Number,
                        x.Address.Street,
                        x.Address.Neighborhood,
                        x.Address.City,
                        x.Address.State,
                        x.Address.Country,
                        x.Address.ZipCode);
                })
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.AggregateId, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());
        }
    }
}