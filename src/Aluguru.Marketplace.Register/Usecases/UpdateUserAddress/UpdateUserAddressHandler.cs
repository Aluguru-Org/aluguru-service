using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Domain.Repositories;
using Aluguru.Marketplace.Register.Dtos;
using System.Threading;
using System.Threading.Tasks;
using Aluguru.Marketplace.Crosscutting.Google;

namespace Aluguru.Marketplace.Register.Usecases.UpadeUserAddress
{
    public class UpdateUserAddressHandler : IRequestHandler<UpdateUserAddressCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IGeocodeService _geocodeService;

        public UpdateUserAddressHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler, IGeocodeService geocodeService)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
            _geocodeService = geocodeService;
        }

        public async Task<bool> Handle(UpdateUserAddressCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<User>();

            var user = await queryRepository.GetUserAsync(command.UserId, false);
            
            if (user == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The User with Id [{command.UserId}] does not exist"));
                return false;
            }

            var response = await _geocodeService.Geocode($"{command.Street}, {command.Number} - {command.Neighborhood}, {command.City} - {command.State}, {command.ZipCode}, {command.Country}");

            if (!response.Success)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Address not found"));
                return false;
            }
            
            var repository = _unitOfWork.Repository<User>();

            var address = new Address(response.Street, response.Number, response.Neighborhood, response.City, response.State, response.Country, response.ZipCode, command.Complement);

            user.UpdateAddress(address);
            repository.Update(user);

            return true;
        }
    }
}
