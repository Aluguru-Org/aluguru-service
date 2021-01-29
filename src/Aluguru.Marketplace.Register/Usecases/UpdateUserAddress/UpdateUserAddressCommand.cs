using FluentValidation;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;
using Aluguru.Marketplace.Register.Domain;

namespace Aluguru.Marketplace.Register.Usecases.UpadeUserAddress
{
    public class UpdateUserAddressCommand : Command<bool>
    {
        public UpdateUserAddressCommand(Guid userId, string street, string number, string neighborhood, 
            string city, string state, string country, string zipCode, string complement)
        {
            UserId = userId;
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
            Complement = complement;
        }

        public Guid UserId { get; }
        public string Street { get; set; }
        public string Number { get; }
        public string Neighborhood { get; }
        public string City { get; }
        public string State { get; }
        public string Country { get; }
        public string ZipCode { get; }
        public string Complement { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateUserAddressCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateUserAddressCommandValidator : AbstractValidator<UpdateUserAddressCommand>
    {
        public UpdateUserAddressCommandValidator()
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
            RuleFor(x => x.ZipCode).NotEmpty();
            RuleFor(x => x.Street).NotEmpty();
            RuleFor(x => x.Number).NotEmpty();
            RuleFor(x => x.Neighborhood).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.State).NotEmpty();
            RuleFor(x => x.Country).NotEmpty();            
        }
    }
}
