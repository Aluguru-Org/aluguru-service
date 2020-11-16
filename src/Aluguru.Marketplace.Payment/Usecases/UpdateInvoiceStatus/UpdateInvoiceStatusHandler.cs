using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Payment.Data.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Payment.Usecases.UpdateInvoiceStatus
{
    public class UpdateInvoiceStatusHandler : IRequestHandler<UpdateInvoiceStatusCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateInvoiceStatusHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateInvoiceStatusCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Domain.Payment>();
            var repository = _unitOfWork.Repository<Domain.Payment>();

            var payment = await queryRepository.GetPaymentByInvoiceIdAsync(command.Id);

            if (command.Status == "paid")
            {
                payment.MarkAsPaid();
            }

            repository.Update(payment);

            return true;
        }
    }
}
