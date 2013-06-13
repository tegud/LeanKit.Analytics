using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.Data;

namespace LeanKit.ReleaseManager.Models.Graphs
{
    public class ActivityBreakDownFactory
    {
        public ActivityBreakdown Build(Ticket ticket)
        {
            var allTicketActivityDurations = ticket.Activities.Select(a => new
                {
                    a.Duration,
                    Title = MapActivityTitle(a.Title)
                }).Where(y => !String.IsNullOrWhiteSpace(y.Title) && y.Duration.Hours > 0);

            var totalActivityHours = (double)allTicketActivityDurations.Sum(t => t.Duration.Hours);

            var activityBreakdownItems = new List<ActivityBreakdownItem>
                {
                    new ActivityBreakdownItem {Activity = "Developing"},
                    new ActivityBreakdownItem {Activity = "Testing"},
                    new ActivityBreakdownItem {Activity = "Waiting to Test", IsWaste = true},
                    new ActivityBreakdownItem {Activity = "Waiting to Release", IsWaste = true},
                    new ActivityBreakdownItem {Activity = "Blocked", IsWaste = true}
                };

            foreach (var ticketActivity in allTicketActivityDurations)
            {
                var activityName = ticketActivity.Title;
                if (activityName.Contains("Blocked"))
                {
                    activityName = "Blocked";
                }

                var activity = activityBreakdownItems.FirstOrDefault(a => a.Activity == activityName);

                if (activity == null)
                {
                    continue;
                }

                activity.Percent += ((ticketActivity.Duration.Hours / totalActivityHours) * 100);
                activity.Hours += ticketActivity.Duration.Hours;
                activity.Days += ticketActivity.Duration.Days;
            }

            var activityBreakdown = new ActivityBreakdown(activityBreakdownItems);
            return activityBreakdown;
        }

        private static string MapActivityTitle(string title)
        {
            if (title.StartsWith("Blocked: "))
            {
                return "Blocked";
            }

            var titleMappings = new Dictionary<string, string>
                {
                    {"DEV WIP", "Developing"},
                    {"DEV Done", "Waiting to Test"},
                    {"READY FOR TEST", "Waiting to Test"},
                    {"TEST WIP", "Testing" },
                    {"READY FOR RELEASE", "Waiting to Release"}
                };


            title = title.ToUpper();
            return titleMappings.ContainsKey(title) ? titleMappings[title] : String.Empty;
        }
    }
}