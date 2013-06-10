using LeanKit.Data.SQL;

namespace LeanKit.ReleaseManager.Models
{
    public class CreateReleaseReleaseRecordFactory : IBuildNewReleaseRecords
    {
        private readonly IParsePlannedReleaseDate _plannedDateParser;
        private readonly IBuildIncludedTicketsForNewReleases _newReleaseIncludedTicketsBuilders;

        public CreateReleaseReleaseRecordFactory(IParsePlannedReleaseDate plannedDateParser, IBuildIncludedTicketsForNewReleases newReleaseIncludedTicketsBuilders)
        {
            _plannedDateParser = plannedDateParser;
            _newReleaseIncludedTicketsBuilders = newReleaseIncludedTicketsBuilders;
        }

        public ReleaseRecord Build(ReleaseInputModel release)
        {
            var plannedDate = _plannedDateParser.ParsePlannedDate(release);
            var includedTicketRecords = _newReleaseIncludedTicketsBuilders.ParseIncludedTickets(release);
            var releaseRecord = new ReleaseRecord
                {
                    PlannedDate = plannedDate,
                    SvnRevision = release.SvnRevision,
                    ServiceNowId = release.ServiceNowId,
                    IncludedTickets = includedTicketRecords,
                    Id = release.Id
                };
            return releaseRecord;
        }
    }
}