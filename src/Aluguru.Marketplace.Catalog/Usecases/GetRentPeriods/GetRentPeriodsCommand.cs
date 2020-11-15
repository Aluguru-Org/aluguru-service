using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Catalog.Usecases.GetRentPeriods
{
    public class GetRentPeriodsCommand : Command<GetRentPeriodsCommandResponse>
    {

    }

    public class GetRentPeriodsCommandResponse
    {
        public List<RentPeriodDTO> RentPeriods { get; set; }
    }
}
