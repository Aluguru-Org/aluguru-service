using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using Aluguru.Marketplace.Payment.Dtos;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Payment.Usecases.GetPayment
{
    public class GetPaymentHandler : IRequestHandler<GetPaymentCommand, GetPaymentCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPaymentHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<GetPaymentCommandResponse> Handle(GetPaymentCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Domain.Payment>();
            var payment = await queryRepository.GetByIdAsync(command.PaymentId);

            return new GetPaymentCommandResponse
            {
                Payment = _mapper.Map<PaymentDTO>(payment)
            };
        }
    }
}
