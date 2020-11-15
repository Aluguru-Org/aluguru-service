using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Rent.Data.Repositories;
using Aluguru.Marketplace.Rent.Domain;
using Aluguru.Marketplace.Rent.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Usecases.GetVouchers
{
    public class GetVouchersHandler : IRequestHandler<GetVouchersCommand, GetVouchersCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetVouchersHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetVouchersCommandResponse> Handle(GetVouchersCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Voucher>();

            var vouchers = await queryRepository.GetVouchersAsync();

            return new GetVouchersCommandResponse()
            {
                Vouchers = _mapper.Map<List<VoucherDTO>>(vouchers)
            };
        }
    }
}