using Aluguru.Marketplace.Communication.Dtos;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;

namespace Aluguru.Marketplace.Catalog.Usecases.DebitProductStock
{
    public class DebitProductStockCommand : Command<bool>
    {
        public DebitProductStockCommand(OrderDTO order)
        {
            Order = order;
        }

        public OrderDTO Order { get; set; }
    }
}
