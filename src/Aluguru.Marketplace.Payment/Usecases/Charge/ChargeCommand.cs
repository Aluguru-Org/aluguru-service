using Aluguru.Marketplace.Communication.Dtos;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;

namespace Aluguru.Marketplace.Payment.Usecases.Charge
{
    public class ChargeCommand : Command<bool>
    {
        public string Token { get; set; }
        public OrderDTO Order { get; set; }
    }
}
