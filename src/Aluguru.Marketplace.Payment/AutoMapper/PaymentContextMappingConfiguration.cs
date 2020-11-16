using Aluguru.Marketplace.Payment.Dtos;
using Aluguru.Marketplace.Payment.Usecases.UpdateInvoiceStatus;
using AutoMapper;

namespace Aluguru.Marketplace.Payment.AutoMapper
{
    public class PaymentContextMappingConfiguration : Profile
    {
        public PaymentContextMappingConfiguration()
        {
            DtoToDomainConfiguration();
            DomainToDtoConfiguration();
        }

        private void DtoToDomainConfiguration()
        {
            CreateMap<InvoiceStatusChangedDTO, UpdateInvoiceStatusCommand>()
                .ConstructUsing(x => new UpdateInvoiceStatusCommand(
                    x.Id,
                    x.AccountId,
                    x.Status))
                .ForMember(x => x.Timestamp, c => c.Ignore())
                .ForMember(x => x.MessageType, c => c.Ignore())
                .ForMember(x => x.ValidationResult, c => c.Ignore());
        }

        private void DomainToDtoConfiguration()
        {
            CreateMap<Domain.Payment, PaymentDTO>()
                .ConstructUsing(x => new PaymentDTO()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    OrderId = x.OrderId,
                    DateCreated = x.DateCreated,
                    PaymentMethod = x.PaymentMethod.ToString(),
                    Identification = x.Identification,
                    InvoiceId = x.InvoiceId,
                    Pdf = x.Pdf,
                    Url = x.Url,
                    Paid = x.Paid,
                });
        }
    }
}
