using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Aluguru.Marketplace.Catalog.Domain
{
    public class Period : ValueObject
    {
        public Period(DateTime start, DateTime end)            
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public bool IsDateBetween(DateTime date)
        {
            return date.Date >= Start.Date && End.Date <= date.Date;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Start.Date;
            yield return End.Date;
        }

        protected override void ValidateValueObject()
        {
            Ensure.That(Start > DateTime.MinValue, "Start date cannot be empty");
            Ensure.That(End > DateTime.MinValue, "End date cannot be empty");
            Ensure.That(End > Start, "End date must be greater than Start date");
        }
    }
}
