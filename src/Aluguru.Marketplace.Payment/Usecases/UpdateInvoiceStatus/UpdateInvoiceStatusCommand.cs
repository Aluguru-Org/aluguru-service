using Aluguru.Marketplace.Infrastructure.Bus.Messages;

namespace Aluguru.Marketplace.Payment.Usecases.UpdateInvoiceStatus
{
    public class UpdateInvoiceStatusCommand : Command<bool>
    {
        public UpdateInvoiceStatusCommand(string id, string accountId, string status)
        {
            Id = id;
            AccountId = accountId;
            Status = status;
        }

        public string Id { get; set; }
        public string AccountId { get; set; }
        public string Status { get; set; }
    }
}
