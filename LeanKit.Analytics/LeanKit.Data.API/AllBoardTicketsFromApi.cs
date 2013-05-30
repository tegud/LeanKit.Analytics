using System.Collections.Generic;
using System.Linq;
using LeanKit.APIClient.API;
using LeanKit.Utilities.Collections;

namespace LeanKit.Data.API
{
    public class AllBoardTicketsFromApi : IAllBoardTicketsRepository
    {
        private readonly IApiCaller _apiCaller;
        private readonly ITicketFactory _ticketFactory;

        public AllBoardTicketsFromApi(IApiCaller apiCaller, 
            ITicketFactory ticketFactory)
        {
            _apiCaller = apiCaller;
            _ticketFactory = ticketFactory;
        }

        public AllTicketsForBoard Get()
        {
            var board = _apiCaller.GetBoard();

            var allTicketsFromBoard = board.Lanes.SelectMany(c => c.Cards).ToList();

            var allArchiveCards = GetArchiveCards().Where(c => !string.IsNullOrWhiteSpace(c.Title) && !c.Title.Contains("Cards older than"));

            allTicketsFromBoard.AddRange(allArchiveCards);

            return new AllTicketsForBoard
                {
                    Tickets = allTicketsFromBoard.Select(card => _ticketFactory.Build(card))
                };
        }

        private IEnumerable<LeankitBoardCard> GetArchiveCards()
        {
            var archive = _apiCaller.GetBoardArchive();
            var allArchiveLanes = archive.FlattenHierarchy(y => y.ChildLanes);

            return allArchiveLanes.SelectMany(z => z.Lane.Cards ?? new List<LeankitBoardCard>().ToArray());
        }
    }
}