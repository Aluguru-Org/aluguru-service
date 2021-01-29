using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Payment.Usecases.UpdateInvoiceStatus
{
    public class UpdateInvoiceStatusCommand : Command<bool>
    {
        public UpdateInvoiceStatusCommand(string @event, Dictionary<string, string> data)
        {
            Event = @event;
            Id = data["id"];
            AccountId = data["account_id"];
            Status = data["status"];
        }
        public string Event { get; set; }
        public string Id { get; set; }
        public string AccountId { get; set; }
        public string Status { get; set; }
    }
}
