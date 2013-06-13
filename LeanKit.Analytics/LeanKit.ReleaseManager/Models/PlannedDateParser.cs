using System;
using LeanKit.ReleaseManager.Models.Releases;

namespace LeanKit.ReleaseManager.Models
{
    public class PlannedDateParser : IParsePlannedReleaseDate
    {
        public DateTime ParsePlannedDate(ReleaseInputModel release)
        {
            var plannedDate = release.PlannedDate;
            var splitTime = release.PlannedTime.Split(':');
            var hours = Int32.Parse(splitTime[0]);
            var minutes = Int32.Parse(splitTime[1]);
            plannedDate = plannedDate.AddHours(hours);
            plannedDate = plannedDate.AddMinutes(minutes);
            return plannedDate;
        }
    }
}