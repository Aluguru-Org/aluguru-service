using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Payment.Data.Repositories
{
    public static class PaymentRepositoryExtensions
    {
        public static async Task<Domain.Payment> GetPaymentByInvoiceIdAsync(this IQueryRepository<Domain.Payment> repository, string invoiceId, bool disableTracking = true)
        {
            return await repository.FindOneAsync(x => x.InvoiceId == invoiceId, null, disableTracking);
        }
    }
}
