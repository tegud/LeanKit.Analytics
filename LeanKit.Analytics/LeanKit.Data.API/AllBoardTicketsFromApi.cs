using System.Collections.Generic;
using System.Linq;
using LeanKit.APIClient.API;
using LeanKit.Utilities.Collections;

namespace LeanKit.Data.API
{
    public class AllBoardTicketsFromApi : IAllBoardTicketsRepository
    {
        private readonly IApiCaller _apiCaller;
        private readonly ICreateTickets _ticketFactory;
        private readonly IValidateLeankitCards _validArchiveCardSpecification;

        public AllBoardTicketsFromApi(IApiCaller apiCaller,
            ICreateTickets ticketFactory, 
            IValidateLeankitCards validArchiveCardSpecification)
        {
            _apiCaller = apiCaller;
            _ticketFactory = ticketFactory;
            _validArchiveCardSpecification = validArchiveCardSpecification;
        }

        public AllTicketsForBoard Get()
        {
            var board = _apiCaller.GetBoard();

            var allTicketsFromBoard = board.Lanes.SelectMany(c => c.Cards).ToList();

            var allArchiveCards = GetArchiveCards().Where(_validArchiveCardSpecification.IsSatisfiedBy);

            allTicketsFromBoard.AddRange(allArchiveCards);

            return new AllTicketsForBoard
                {
                    Tickets = allTicketsFromBoard.Select(card => _ticketFactory.Build(card)),
                    Lanes = board.Lanes.Select((l,i) => new Activity
                        {
                            Id = l.Id,
                            Title = l.Title,
                            Index = i
                        })
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