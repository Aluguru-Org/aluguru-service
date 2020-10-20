using Aluguru.Marketplace.Catalog.ViewModels;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Catalog.Usecases.GetRentPeriods
{
    public class GetRentPeriodsCommand : Command<GetRentPeriodsCommandResponse>
    {

    }

    public class GetRentPeriodsCommandResponse
    {
        public List<RentPeriodViewModel> RentPeriods { get; set; }
    }
}
