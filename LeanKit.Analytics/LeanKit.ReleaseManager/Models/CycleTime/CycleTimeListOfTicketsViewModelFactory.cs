using System.Collections.Generic;
using System.Linq;
using LeanKit.Data;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Models.CycleTime
{
    public class CycleTimeListOfTicketsViewModelFactory : IBuildListOfCycleTimeItems
    {
        private readonly IMakeCycleTimeReleaseViewModels _cycleTimeReleaseViewModelFactory;

        public CycleTimeListOfTicketsViewModelFactory(IMakeCycleTimeReleaseViewModels cycleTimeReleaseViewModelFactory)
        {
            _cycleTimeReleaseViewModelFactory = cycleTimeReleaseViewModelFactory;
        }

        public IEnumerable<CycleTimeTicketItem> Build(IEnumerable<Ticket> tickets)
        {
            return tickets.Select(t => new CycleTimeTicketItem
                {
                    Id = t.Id,
                    ExternalId = t.ExternalId,
                    Title = t.Title,
                    StartedFriendlyText = DateFriendTextExtensions.ToFriendlyText(t.Started, "dd MMM yyyy", " HH:mm"),
                    Release = _cycleTimeReleaseViewModelFactory.Build(t.Release),
                    FinishedFriendlyText = DateFriendTextExtensions.ToFriendlyText(t.Finished, "dd MMM yyyy", " HH:mm"),
                    Duration = GetDurationText(t),
                    Size = GetSize(t)
                });
        }

        private static string GetSize(Ticket t)
        {
            if (t.Size == 0)
            {
                return "?";
            }

            return t.Size.ToString();
        }

        private static string GetDurationText(Ticket ticket)
        {
            var isPlural = ticket.CycleTime.Days != 1;
            var suffix = (isPlural ? "s" : "");

            return string.Format("{0} Day{1}", ticket.CycleTime.Days, suffix);
        }
    }
}