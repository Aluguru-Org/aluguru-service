using FluentValidation;
using Aluguru.Marketplace.Catalog.ViewModels;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Catalog.Usecases.CreateRentPeriod
{
    public class CreateRentPeriodCommand : Command<CreateRentPeriodCommandResponse>
    {
        public CreateRentPeriodCommand(string name, int days)
        {
            Name = name;
            Days = days;
        }

        public string Name { get; set; }
        public int Days { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new CreateRentPeriodCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateRentPeriodCommandValidator : AbstractValidator<CreateRentPeriodCommand>
    {
        public CreateRentPeriodCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Days).GreaterThan(0);
        }
    }

    public class CreateRentPeriodCommandResponse 
    {
        public RentPeriodViewModel RentPeriod { get; set; }
    }
}