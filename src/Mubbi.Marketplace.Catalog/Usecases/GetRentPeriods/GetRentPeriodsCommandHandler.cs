using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mubbi.Marketplace.Catalog.Data.Repositories;

namespace Mubbi.Marketplace.Catalog.Usecases.GetRentPeriods
{
    public class GetRentPeriodsCommandHandler : IRequestHandler<GetRentPeriodsCommand, GetRentPeriodsCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRentPeriodsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetRentPeriodsCommandResponse> Handle(GetRentPeriodsCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<RentPeriod>();

            var rentPeriods = await queryRepository.GetRentPeriodsAsync();

            return new GetRentPeriodsCommandResponse()
            {
                RentPeriods = _mapper.Map<List<RentPeriodViewModel>>(rentPeriods)
            };
        }
    }
}