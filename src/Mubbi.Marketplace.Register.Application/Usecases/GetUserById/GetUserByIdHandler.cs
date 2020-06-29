﻿using MediatR;
using Mubbi.Marketplace.Infrastructure.Data;
using Mubbi.Marketplace.Register.Application.ViewModels;
using Mubbi.Marketplace.Register.Domain.Models;
using Mubbi.Marketplace.Register.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Register.Application.Usecases.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdCommand, GetUserByIdCommandResponse>
    {
        private readonly IEfUnitOfWork _unitOfWork;

        public GetUserByIdHandler(IEfUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetUserByIdCommandResponse> Handle(GetUserByIdCommand command, CancellationToken cancellationToken)
        {
            var userRepository = _unitOfWork.QueryRepository<User>();

            var user = await userRepository.GetUserAsync(command.UserId);

            return new GetUserByIdCommandResponse
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
