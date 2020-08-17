using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Rent.Data.Repositories;
using Mubbi.Marketplace.Rent.Domain;
using Mubbi.Marketplace.Rent.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Rent.Usecases.CreateVoucher
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
                Voucher = _mapper.Map<VoucherViewModel>(voucher)
            };
        }
    }
}