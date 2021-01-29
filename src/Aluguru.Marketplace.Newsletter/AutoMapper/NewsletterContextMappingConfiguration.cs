using Aluguru.Marketplace.Newsletter.Domain;
using Aluguru.Marketplace.Newsletter.Dtos;
using AutoMapper;

namespace Aluguru.Marketplace.Newsletter.AutoMapper
{
    public class NewsletterContextMappingConfiguration : Profile
    {
        public NewsletterContextMappingConfiguration()
        {
            ViewModelToDomainConfiguration();
            DomainToViewModelConfiguration();
        }

        private void ViewModelToDomainConfiguration()
        {
            CreateMap<SubscriberDto, Subscriber>()
                .ConstructUsing(x => new Subscriber(x.Email))
                .ForMember(x => x.Id, c => c.Ignore())
                .ForMember(x => x.DateCreated, c => c.Ignore())
                .ForMember(x => x.DateUpdated, c => c.Ignore());
        }

        private void DomainToViewModelConfiguration()
        {
            CreateMap<Subscriber, SubscriberDto>();
        }
    }
}
