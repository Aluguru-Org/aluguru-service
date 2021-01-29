using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Rent.Dtos;
using System;
using Aluguru.Marketplace.Domain;

namespace Aluguru.Marketplace.Rent.Usecases.GetRevenue
{
    public class GetRevenueCommand : Command<GetRevenueCommandResponse>
    {
        public GetRevenueCommand(DateTime startDate, DateTime endDate, Guid? companyId)
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
            ValidationResult = new GetRevenueCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class GetRevenueCommandValidator : AbstractValidator<GetRevenueCommand>
    {
        public GetRevenueCommandValidator()
        {
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty();
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
        }
    }

    public class GetRevenueCommandResponse
    {
        public decimal Revenue { get; set; }
    }
}
