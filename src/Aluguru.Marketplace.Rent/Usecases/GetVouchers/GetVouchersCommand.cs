using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.ViewModels;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Rent.Usecases.GetVouchers
{
    public class GetVouchersCommand : Command<GetVouchersCommandResponse>
    {
        public GetVouchersCommand() { }
    }

    public class GetVouchersCommandResponse
    {
        public List<VoucherViewModel> Vouchers { get; set; }
    }
}