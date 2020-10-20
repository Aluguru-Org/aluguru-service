using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Catalog.ViewModels;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Usecases.CreateRentPeriod
{
    public class CreateRentPeriodHandler : IRequestHandler<CreateRentPeriodCommand, CreateRentPeriodCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public CreateRentPeriodHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<CreateRentPeriodCommandResponse> Handle(CreateRentPeriodCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<RentPeriod>();
            var rentPeriod = await queryRepository.GetRentPeriodByNameAsync(request.Name);

            if (rentPeriod != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"The RentPeriod {rentPeriod} is already registered"));
                return new CreateRentPeriodCommandResponse();
            }

            var repository = _unitOfWork.Repository<RentPeriod>();

            rentPeriod = new RentPeriod(request.Name, request.Days);

            rentPeriod = await repository.AddAsync(rentPeriod);

            return new CreateRentPeriodCommandResponse
            {
                RentPeriod = _mapper.Map<RentPeriodViewModel>(rentPeriod)
            };
        }
    }
}
