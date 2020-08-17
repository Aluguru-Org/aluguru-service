using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Rent.Data.Repositories;
using Mubbi.Marketplace.Rent.Domain;
using Mubbi.Marketplace.Rent.ViewModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Rent.Usecases.GetVouchers
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
                Vouchers = _mapper.Map<List<VoucherViewModel>>(vouchers)
            };
        }
    }
}