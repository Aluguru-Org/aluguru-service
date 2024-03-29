﻿using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Usecases.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public UpdateProductHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            if (command.ProductId != command.Product.Id)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The provided Product Id [{command.ProductId}] does not match with the Product passed to be updated [{command.Product.Id}]"));
                return new UpdateProductCommandResponse();
            }

            var queryRepository = _unitOfWork.QueryRepository<Product>();

            var existed = await queryRepository.GetProductAsync(command.ProductId, false);

            if (existed == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The Category {existed} was not found"));
                return new UpdateProductCommandResponse();
            }

            var repository = _unitOfWork.Repository<Product>();

            var updated = existed.UpdateProduct(command);
            var category = repository.Update(updated);

            return new UpdateProductCommandResponse()
            {
                Product = _mapper.Map<ProductDTO>(category)
            };
        }
    }
}
