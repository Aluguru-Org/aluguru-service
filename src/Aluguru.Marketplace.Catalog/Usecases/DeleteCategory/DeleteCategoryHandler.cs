using MediatR;
using Microsoft.EntityFrameworkCore;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Usecases.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;

        public DeleteCategoryHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Category>();

            var category = await queryRepository.GetByIdAsync(command.CategoryId, category => category.Include(x => x.SubCategories));

            if (category == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The category {category} was not found"));
                return false;
            }

            var repository = _unitOfWork.Repository<Category>();

            repository.Delete(category);

            return true;
        }
    }
}
