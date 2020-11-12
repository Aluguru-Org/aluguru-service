using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using Aluguru.Marketplace.Newsletter.Domain;
using Aluguru.Marketplace.Newsletter.ViewModels;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Newsletter.Services
{
    public interface INewsletterService
    {
        Task<List<string>> GetAllSubscribers();
        Task<SubscriberViewModel> AddSubscriber(SubscriberViewModel subscriber);
        Task<bool> RemoveSubscriber(SubscriberViewModel subscriber);
    }

    public class NewsletterService : INewsletterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NewsletterService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<string>> GetAllSubscribers()
        {
            var subscribers = await _unitOfWork.QueryRepository<Subscriber>().ListAsync(null);

            return subscribers.Select(x => x.Email).ToList();
        }

        public async Task<SubscriberViewModel> AddSubscriber(SubscriberViewModel subscriberViewModel)
        {
            var subscriber = _mapper.Map<Subscriber>(subscriberViewModel);

            subscriber = await _unitOfWork.Repository<Subscriber>().AddAsync(subscriber);

            await _unitOfWork.SaveChangesAsync(default);

            return _mapper.Map<SubscriberViewModel>(subscriber);
        }
        

        public async Task<bool> RemoveSubscriber(SubscriberViewModel subscriberViewModel)
        {
            var subscriber = await _unitOfWork.QueryRepository<Subscriber>().FindOneAsync(
                x => x.Email == subscriberViewModel.Email
            );

            if (subscriber != null)
            {
                _unitOfWork.Repository<Subscriber>().Delete(subscriber);

                return await _unitOfWork.SaveChangesAsync(default) == 1;
            }

            return false;
        }
    }
}
