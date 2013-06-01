using System;

namespace LeanKit.Data
{
    public interface ICalculateWorkDuration
    {
        WorkDuration CalculateDuration(DateTime start, DateTime end);
    }
}