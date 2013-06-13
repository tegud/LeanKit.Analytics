using System.Collections.Generic;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models.Releases;

namespace LeanKit.ReleaseManager.Models
{
    public interface IBuildIncludedTicketsForNewReleases
    {
        List<IncludedTicketRecord> ParseIncludedTickets(ReleaseInputModel release);
    }
}