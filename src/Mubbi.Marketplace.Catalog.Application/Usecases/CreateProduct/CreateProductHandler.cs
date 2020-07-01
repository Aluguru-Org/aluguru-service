using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Application.Usecases.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateProductHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<CreateProductCommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
