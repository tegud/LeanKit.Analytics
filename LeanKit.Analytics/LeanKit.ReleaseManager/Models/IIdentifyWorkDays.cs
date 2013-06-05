using System;

namespace LeanKit.ReleaseManager.Models
{
    public interface IIdentifyWorkDays
    {
        bool IsSatisfiedBy(DateTime date);
    }
}