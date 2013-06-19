using System.Collections.Generic;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Models.TimePeriods
{
    public class ProductOwnerDashboardTimePeriodConfiguration : IConfigureTimePeriods
    {
        private readonly IKnowTheCurrentDateAndTime _dateTimeWrapper;

        public ProductOwnerDashboardTimePeriodConfiguration(IKnowTheCurrentDateAndTime dateTimeWrapper)
        {
            _dateTimeWrapper = dateTimeWrapper;
        }

        public IEnumerable<IMatchATimePeriod> Matchers
        {
            get
            {
                return new IMatchATimePeriod[]
                    {
                        new MatchWeekCommencingTimePeriod(_dateTimeWrapper),
                        new MatchKeywordTimePeriod(_dateTimeWrapper)
                    };
            }
        }

        public string DefaultValue { get { return "this-week"; } }
    }
}