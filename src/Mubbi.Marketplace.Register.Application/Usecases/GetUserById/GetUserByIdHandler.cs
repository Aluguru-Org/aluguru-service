using MediatR;
using Mubbi.Marketplace.Infrastructure.Data;
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
                User = new ViewModels.UserViewModel()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Role = user.Role.ToString(),
                    FirstName = user.Name.FirstName,
                    LastName = user.Name.LastName,
                    Email = user.Email.Address,
                    AddressViewModel = new ViewModels.AddressViewModel
                    {
                        Number = user.Address.Number,
                        Street = user.Address.Street,
                        Neighborhood = user.Address.Neighborhood,
                        City = user.Address.City,
                        State = user.Address.State,
                        Country = user.Address.Country,
                        ZipCode = user.Address.ZipCode
                    },
                    DocumentViewModel = new ViewModels.DocumentViewModel
                    {
                        DocumentType = user.Document.DocumentType.ToString(),
                        Number = user.Document.Number
                    }                    
                }
            };
        }
    }
}
