using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.Dtos;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Rent.Usecases.GetVouchers
{
    public class GetVouchersCommand : Command<GetVouchersCommandResponse>
    {
        public GetVouchersCommand() { }
    }

    public class GetVouchersCommandResponse
    {
        public List<VoucherDTO> Vouchers { get; set; }
    }
}