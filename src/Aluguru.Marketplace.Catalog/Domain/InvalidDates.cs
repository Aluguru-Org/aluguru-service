using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static PampaDevs.Utils.Helpers.DateTimeHelper;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Aluguru.Marketplace.Catalog.Domain
{
    public class InvalidDates : Entity
    {
        private readonly List<DayOfWeek> _days;
        private readonly List<DateTime> _dates;
        private readonly List<Period> _periods;

        public InvalidDates(List<DayOfWeek> days, List<DateTime> dates, List<Period> periods)
            : base(NewId())
        {
            _days = new List<DayOfWeek>(days);
            _dates = new List<DateTime>(dates);
            _periods = new List<Period>(periods);
        }

        protected InvalidDates()
            : base(NewId())
        {
            _days = new List<DayOfWeek>();
            _dates = new List<DateTime>();
            _periods = new List<Period>();
        }

        public Guid ProductId { get; set; }

        public IReadOnlyCollection<DayOfWeek> Days => _days;
        public IReadOnlyCollection<DateTime> Dates => _dates;
        public IReadOnlyCollection<Period> Periods => _periods;

        // EF Relational
        public virtual Product Product { get; set; }

        public bool HasDateAvaiabilityFor(DateTime date)
        {
            if (_days.Any(x => x == date.DayOfWeek)) return false;

            if (_dates.Any(x => x.Date == date.Date)) return false;

            if (_periods.Any(x => x.IsDateBetween(date))) return false;

            return true;
        }

        public void Update(List<DayOfWeek> days, List<DateTime> dates, List<PeriodDTO> periods)
        {
            _days.Clear();
            _dates.Clear();
            _periods.Clear();

            _days.AddRange(days);
            _dates.AddRange(dates);

            foreach(var period in periods)
            {
                _periods.Add(new Period(period.Start, period.End));
            }

            ValidateEntity();

            DateUpdated = NewDateTime();
        }

        protected override void ValidateEntity()
        {
            Ensure.That(_days.Count() == _days.Distinct().Count(), "You cannot have repeated week days");
            Ensure.That(_dates.Count() == _dates.Distinct().Count(), "You cannot have repeated dates");
            Ensure.That(_periods.Count() == _periods.Distinct().Count(), "You cannot have repeated periods");
        }
    }
}
