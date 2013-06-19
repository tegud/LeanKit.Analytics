using System.Collections.Generic;

namespace LeanKit.ReleaseManager.Models.TimePeriods
{
    public interface IConfigureTimePeriods
    {
        IEnumerable<IMatchATimePeriod> Matchers { get; }
        string DefaultValue { get; }
    }
}