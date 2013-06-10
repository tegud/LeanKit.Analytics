using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.Data.SQL;

namespace LeanKit.ReleaseManager.Models
{
    public class NewReleaseIncludedTicketsBuilders : IBuildIncludedTicketsForNewReleases
    {
        public List<IncludedTicketRecord> ParseIncludedTickets(ReleaseInputModel release)
        {
            var includedTicketRecords = release.SelectedTickets.Split(',').Select(ticketId => new IncludedTicketRecord
                {
                    CardId = Int32.Parse((ticketId))
                }).ToList();
            return includedTicketRecords;
        }
    }
}