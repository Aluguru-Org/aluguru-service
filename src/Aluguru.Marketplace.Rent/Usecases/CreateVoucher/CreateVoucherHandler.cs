using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Rent.Data.Repositories;
using Aluguru.Marketplace.Rent.Domain;
using Aluguru.Marketplace.Rent.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Usecases.CreateVoucher
{
    public class CreateVoucherHandler : IRequestHandler<CreateVoucherCommand, CreateVoucherCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public CreateVoucherHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<CreateVoucherCommandResponse> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Voucher>();

            var voucher = await queryRepository.GetVoucherAsync(request.Voucher.Code);

            if (voucher != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"Voucher {request.Voucher.Code} already exists"));
                return default;
            }

            voucher = new Voucher(
                request.Voucher.Code,
                (EVoucherType)Enum.Parse(typeof(EVoucherType), request.Voucher.VoucherType),
                request.Voucher.Discount,
                request.Voucher.Amount,
                request.Voucher.ExpirationDate);

            var repository = _unitOfWork.Repository<Voucher>();

            voucher = await repository.AddAsync(voucher);

            return new CreateVoucherCommandResponse()
            {
                Voucher = _mapper.Map<VoucherDTO>(voucher)
            };
        }
    }
}