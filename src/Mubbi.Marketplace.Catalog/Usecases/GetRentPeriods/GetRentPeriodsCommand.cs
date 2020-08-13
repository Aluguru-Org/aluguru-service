using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Catalog.Usecases.GetRentPeriods
{
    public class GetRentPeriodsCommand : Command<GetRentPeriodsCommandResponse>
    {

    }

    public class GetRentPeriodsCommandResponse
    {
        public List<RentPeriodViewModel> RentPeriods { get; set; }
    }
}
