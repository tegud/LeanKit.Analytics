using System.Collections.Generic;
using LeanKit.Data.SQL;

namespace LeanKit.ReleaseManager.Models
{
    public interface IBuildIncludedTicketsForNewReleases
    {
        List<IncludedTicketRecord> ParseIncludedTickets(NewReleaseViewModel release);
    }
}