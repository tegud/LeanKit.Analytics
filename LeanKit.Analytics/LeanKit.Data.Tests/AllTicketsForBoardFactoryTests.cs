using System.Collections.Generic;
using System.Linq;
using LeanKit.APIClient.API;
using LeanKit.Data.API;
using NUnit.Framework;

namespace LeanKit.Data.Tests
{
    [TestFixture]
    public class AllTicketsForBoardFactoryTests : IApiCaller, ICreateTickets, IValidateLeankitCards
    {
        private LeankitBoardCard _actualLeankitBoardCard;
        private LeankitBoardCard _expectedLeankitBoardCard;
        private LeankitBoardCard _expectedArchiveLeankitBoardCard;
        private bool _meetsSpecification;

        [Test]
        public void AddsTicketFromBoardColumns()
        {
            IApiCaller apiCaller = this;
            ICreateTickets ticketFactory = this;
            IValidateLeankitCards validArchiveCardSpecification = this;

            _meetsSpecification = true;
            _expectedArchiveLeankitBoardCard = null;
            _expectedLeankitBoardCard = new LeankitBoardCard();

            new AllBoardTicketsFromApi(apiCaller, ticketFactory, validArchiveCardSpecification).Get().Tickets.ToArray();

            Assert.That(_actualLeankitBoardCard, Is.EqualTo(_expectedLeankitBoardCard));
        }

        [Test]
        public void AddsTicketFromArchiveColumns()
        {
            IApiCaller apiCaller = this;
            ICreateTickets ticketFactory = this;
            IValidateLeankitCards validArchiveCardSpecification = this;

            _meetsSpecification = true;
            _expectedArchiveLeankitBoardCard = new LeankitBoardCard();

            new AllBoardTicketsFromApi(apiCaller, ticketFactory, validArchiveCardSpecification).Get().Tickets.ToArray();

            Assert.That(_actualLeankitBoardCard, Is.EqualTo(_expectedArchiveLeankitBoardCard));
        }

        [Test]
        public void IgnoresArchiveTicketsThatDoNotMatchValidSpecification()
        {
            IApiCaller apiCaller = this;
            ICreateTickets ticketFactory = this;
            IValidateLeankitCards validArchiveCardSpecification = this;

            _meetsSpecification = false;
            _actualLeankitBoardCard = null;
            _expectedLeankitBoardCard = null;
            _expectedArchiveLeankitBoardCard = new LeankitBoardCard();

            new AllBoardTicketsFromApi(apiCaller, ticketFactory, validArchiveCardSpecification).Get().Tickets.ToArray();

            Assert.That(_actualLeankitBoardCard, Is.Null);
        }

        [Test]
        public void AddsBoardActivityTitle()
        {
            IApiCaller apiCaller = this;
            ICreateTickets ticketFactory = this;
            IValidateLeankitCards validArchiveCardSpecification = this;

            _meetsSpecification = false;
            _actualLeankitBoardCard = null;
            _expectedLeankitBoardCard = null;
            _expectedArchiveLeankitBoardCard = new LeankitBoardCard();

            var lanes = new AllBoardTicketsFromApi(apiCaller, ticketFactory, validArchiveCardSpecification).Get().Lanes.ToArray();

            Assert.That(lanes.First().Title, Is.EqualTo("Dev WIP"));
        }

        [Test]
        public void AddsBoardActivityId()
        {
            IApiCaller apiCaller = this;
            ICreateTickets ticketFactory = this;
            IValidateLeankitCards validArchiveCardSpecification = this;

            _meetsSpecification = false;
            _actualLeankitBoardCard = null;
            _expectedLeankitBoardCard = null;
            _expectedArchiveLeankitBoardCard = new LeankitBoardCard();

            var lanes = new AllBoardTicketsFromApi(apiCaller, ticketFactory, validArchiveCardSpecification).Get().Lanes.ToArray();

            Assert.That(lanes.First().Id, Is.EqualTo(1234));
        }

        [TestCase(0)]
        [TestCase(1)]
        public void AddsBoardActivitityIndex(int index)
        {
            IApiCaller apiCaller = this;
            ICreateTickets ticketFactory = this;
            IValidateLeankitCards validArchiveCardSpecification = this;

            _meetsSpecification = false;
            _actualLeankitBoardCard = null;
            _expectedLeankitBoardCard = null;
            _expectedArchiveLeankitBoardCard = new LeankitBoardCard();

            var lanes = new AllBoardTicketsFromApi(apiCaller, ticketFactory, validArchiveCardSpecification).Get().Lanes.ToArray();

            Assert.That(lanes.ElementAt(index).Index, Is.EqualTo(index));
        }

        public LeankitBoard GetBoard()
        {
            return new LeankitBoard
                {
                    Lanes = new[]
                        {
                            new LeankitBoardLane
                                {
                                    Id = 1234,
                                    Title = "Dev WIP",
                                    Cards = new[]
                                        {
                                            _expectedLeankitBoardCard
                                        }
                                },
                            new LeankitBoardLane
                                {
                                    Title = "Dev Done",
                                    Cards = new LeankitBoardCard[0]
                                }
                        }
                };
        }

        public IEnumerable<LeanKitCardHistory> GetCardHistory(int cardId)
        {
            return null;
        }

        public LeankitBoardLaneWrapper GetBoardArchive()
        {
            return new LeankitBoardLaneWrapper
                {
                    Lane = new LeankitBoardLane
                        {
                            Cards = new[]
                                {
                                    _expectedArchiveLeankitBoardCard
                                }
                        }
                };
        }

        public Ticket Build(LeankitBoardCard card)
        {
            if (card != null)
            {
                _actualLeankitBoardCard = card;
            }

            return null;
        }

        public bool IsSatisfiedBy(LeankitBoardCard card)
        {
            return _meetsSpecification;
        }
    }
}
