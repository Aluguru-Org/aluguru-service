using Aluguru.Marketplace.Communication.IntegrationEvents;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Payment.Data.Repositories;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Domain.Repositories;
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
            var paymentQueryRepository = _unitOfWork.QueryRepository<Domain.Payment>();
            var userQueryRepository = _unitOfWork.QueryRepository<User>();

            var repository = _unitOfWork.Repository<Domain.Payment>();            

            var payment = await paymentQueryRepository.GetPaymentByInvoiceIdAsync(command.Id);
            var user = await userQueryRepository.GetUserAsync(payment.UserId);

            if (command.Status == "paid")
            {
                payment.MarkAsPaid();
            }

            payment.AddEvent(new OrderPaidEvent(payment.OrderId, user.FullName, user.Email));

            repository.Update(payment);

            return true;
        }
    }
}
