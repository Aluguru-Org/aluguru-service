using AutoMapper;
using Aluguru.Marketplace.Register.Usecases.CreateUser;
using Aluguru.Marketplace.Register.Usecases.LogInBackofficeClient;
using Aluguru.Marketplace.Register.Dtos;
using Aluguru.Marketplace.Register.Domain;
using System;
using System.Collections.Generic;
using Aluguru.Marketplace.Register.Usecases.CreateUserRole;
using Aluguru.Marketplace.Register.Usecases.UpadeUserName;
using Aluguru.Marketplace.Register.Usecases.UpadeUserContact;
using Aluguru.Marketplace.Register.Usecases.UpadeUserDocument;
using Aluguru.Marketplace.Register.Usecases.UpadeUserAddress;

namespace Aluguru.Marketplace.Register.AutoMapper
{
    public class RegisterContextMappingConfiguration : Profile
    {
        public RegisterContextMappingConfiguration()
        {
            ViewModelToDomainConfiguration();
            DomainToViewModelConfiguration();
        }

        private void ViewModelToDomainConfiguration()
        {
            CreateMap<ContactDTO, Contact>()
                .ConstructUsing((request, context) =>
                {
                    return new Contact(request.Name, request.PhoneNumber, request.Email);
                })
                .ForMember(x => x.User, c => c.Ignore())
                .ForMember(x => x.DateCreated, c => c.Ignore())
                .ForMember(x => x.DateUpdated, c => c.Ignore());

            CreateMap<DocumentDTO, Document>()
                .ConstructUsing((request, context) =>
                {
                    var documentType = (EDocumentType)Enum.Parse(typeof(EDocumentType), request.DocumentType);
                    return new Document(request.Number, documentType);
                })
                .ForMember(x => x.User, c => c.Ignore())
                .ForMember(x => x.DateCreated, c => c.Ignore())
                .ForMember(x => x.DateUpdated, c => c.Ignore());

            CreateMap<AddressDTO, Address>()
                .ConstructUsing((request, context) =>
                {
                    return new Address(request.Street, request.Number, request.Neighborhood, request.City, request.State, request.Country, request.ZipCode, request.Complement);
                })
                .ForMember(x => x.User, c => c.Ignore())
                .ForMember(x => x.DateCreated, c => c.Ignore())
                .ForMember(x => x.DateUpdated, c => c.Ignore());

            CreateMap<CreateUserRoleDTO, CreateUserRoleCommand>()
                .ConstructUsing((request, context) =>
                {
                    var userClaims = context.Mapper.Map<List<UserClaim>>(request.UserClaims);
                    return new CreateUserRoleCommand(request.Name, userClaims);
                })
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());

            CreateMap<CreateUserClaimDTO, UserClaim>()
                .ForMember(x => x.UserRole, c => c.Ignore())
                .ForMember(x => x.DateCreated, c => c.Ignore())
                .ForMember(x => x.DateUpdated, c => c.Ignore());

            CreateMap<LoginUserDTO, LogInUserBackofficeCommand>()
                .ConstructUsing(x => new LogInUserBackofficeCommand(x.Email, x.Password))
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());

            CreateMap<UserRegistrationDTO, CreateUserCommand>()
                .ConstructUsing((x, rc) =>
                {
                    var address = rc.Mapper.Map<Address>(x.Address);
                    var document = rc.Mapper.Map<Document>(x.Document);
                    var contact = rc.Mapper.Map<Contact>(x.Contact);
                    return new CreateUserCommand(x.FullName, x.Password, x.Email, x.Role, document, address, contact);
                })
                .ForMember(x => x.Address, c => c.Ignore())
                .ForMember(x => x.Contact, c => c.Ignore())
                .ForMember(x => x.Document, c => c.Ignore())
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());
        }

        private void DomainToViewModelConfiguration()
        {
            CreateMap<UserClaim, UserClaimDTO>();
            CreateMap<UserRole, UserRoleDTO>();

            CreateMap<Address, AddressDTO>();
            CreateMap<Contact, ContactDTO>();
            CreateMap<Document, DocumentDTO>()
                .ForMember(x => x.DocumentType, opt => opt.MapFrom(c => c.DocumentType.ToString()));
            CreateMap<User, UserDTO>()
                .ConstructUsing((x, rc) =>
                {
                    var contact = rc.Mapper.Map<ContactDTO>(x.Contact);
                    var document = rc.Mapper.Map<DocumentDTO>(x.Document);
                    var address = rc.Mapper.Map<AddressDTO>(x.Address);

                    return new UserDTO()
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        Email = x.Email,
                        Contact = contact,
                        Address = address,
                        Document = document
                    };
                });
        }

    }
}