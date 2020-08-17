using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Rent.ViewModels;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Rent.Usecases.GetVouchers
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