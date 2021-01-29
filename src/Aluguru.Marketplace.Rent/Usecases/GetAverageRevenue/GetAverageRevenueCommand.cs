using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Rent.Usecases.GetAverageRevenue
{
    public class GetAverageRevenueCommand : Command<GetAverageRevenueCommandResponse>
    {
        public GetAverageRevenueCommand(DateTime startDate, DateTime endDate, Guid? companyId)
        {
            StartDate = startDate;
            EndDate = endDate;
            CompanyId = companyId;
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? CompanyId { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new GetAverageRevenueCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class GetAverageRevenueCommandValidator : AbstractValidator<GetAverageRevenueCommand>
    {
        public GetAverageRevenueCommandValidator()
        {
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty();
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
        }
    }

    public class GetAverageRevenueCommandResponse
    {
        public decimal Revenue { get; set; }
    }
}
