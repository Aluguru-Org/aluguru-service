using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Register.Application.ViewModels;
using Mubbi.Marketplace.Register.Domain;
using Mubbi.Marketplace.Register.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Register.Application.Usecases.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUserHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
                User = _mapper.Map<UserViewModel>(user)
            };
        }
    }
}
