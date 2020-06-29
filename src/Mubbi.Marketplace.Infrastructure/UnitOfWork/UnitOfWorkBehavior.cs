using MediatR;
using Mubbi.Marketplace.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Infrastructure.UnitOfWork
{
    public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var result = await next();
            _unitOfWork.SaveChanges();
            return result;
        }
    }
}
