using System.Collections.Generic;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Models.TimePeriods
{
    public class CycleTimeTimePeriodConfiguration : IConfigureTimePeriods
    {
        private readonly IKnowTheCurrentDateAndTime _dateTimeWrapper;

        public CycleTimeTimePeriodConfiguration(IKnowTheCurrentDateAndTime dateTimeWrapper)
        {
            _dateTimeWrapper = dateTimeWrapper;
        }

        public IEnumerable<IMatchATimePeriod> Matchers
        {
            get
            {
                return new IMatchATimePeriod[]
                    {
                        new MatchDaysBeforeTimePeriod(_dateTimeWrapper), 
                        new MatchKeywordTimePeriod(_dateTimeWrapper)
                    };
            }
        }

        public string DefaultValue { get { return "30"; } }
    }
}