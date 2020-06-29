using MediatR;
using Mubbi.Marketplace.Infrastructure.Data;
using Mubbi.Marketplace.Register.Application.ViewModels;
using Mubbi.Marketplace.Register.Domain;
using Mubbi.Marketplace.Register.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Register.Application.Usecases.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
    {
        private readonly IEfUnitOfWork _unitOfWork;

        public CreateUserHandler(IEfUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userRepository = _unitOfWork.Repository<User>();

            var role = (ERoles)Enum.Parse(typeof(ERoles), request.Role);

            var email = new Email(request.Email);

            var address = new Address(
                request.AddressStreet, 
                request.AddressNumber, 
                request.AddressNeighborhood, 
                request.AddressCity, 
                request.AddressState, 
                request.AddressCountry, 
                request.AddressZipCode);

            var document = new Document(request.DocumentNumber, (EDocumentType)Enum.Parse(typeof(EDocumentType), request.DocumentType));
            
            var user = new User(email, request.Password, role, request.FullName, address, document);

            user = await userRepository.AddAsync(user);

            return new CreateUserCommandResponse()
            {
                User = new UserViewModel()
                {
                    Id = user.Id,
                    Role = user.Role.ToString(),
                    FullName = user.FullName,
                    Email = user.Email.Address,
                    Address = new ViewModels.AddressViewModel
                    {
                        Number = user.Address.Number,
                        Street = user.Address.Street,
                        Neighborhood = user.Address.Neighborhood,
                        City = user.Address.City,
                        State = user.Address.State,
                        Country = user.Address.Country,
                        ZipCode = user.Address.ZipCode
                    },
                    Document = new ViewModels.DocumentViewModel
                    {
                        DocumentType = user.Document.DocumentType.ToString(),
                        Number = user.Document.Number
                    }
                }
            };
        }
    }
}
