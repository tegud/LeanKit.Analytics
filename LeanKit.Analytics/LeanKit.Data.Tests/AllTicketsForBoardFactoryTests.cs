using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeanKit.APIClient.API;
using LeanKit.Data.Activities;
using NUnit.Framework;

namespace LeanKit.Data.Tests
{
    [TestFixture]
    public class AllTicketsForBoardFactoryTests : IApiCaller
    {
        private int _expectedId;
        private string _expectedTitle;
        private LeanKitCardHistory[] _cardHistory = new[] 
                {
                    new LeanKitCardHistory
                        {
                            Type = "CardCreationEventDTO",
                            DateTime = "15/02/2013 at 10:50:35 AM",
                            ToLaneTitle = "DEV WIP"
                        }
                };

        private LeankitBoardLaneWrapper _boardArchive = new LeankitBoardLaneWrapper
            {
                Lane = new LeankitBoardLane(),
                ChildLanes = new LeankitBoardLaneWrapper[] {}
            };

        [Test]
        public void SetsTicketId()
        {
            _expectedId = 1234;
            var apiCaller = this;
            Assert.That(new AllTicketsForBoardFactory(apiCaller, new ActivityIsInProgressSpecification(), new TicketActivityFactory()).Build().Tickets.First().Id, Is.EqualTo(_expectedId));
        }

        [Test]
        public void SetsTicketIdFromArchiveTicket()
        {
            _boardArchive = new LeankitBoardLaneWrapper
                {
                    Lane = new LeankitBoardLane
                        {
                            Title = "Archive"
                        },
                    ChildLanes = new[]
                        {
                            new LeankitBoardLaneWrapper
                                {
                                    Lane = new LeankitBoardLane
                                        {
                                            Title = "Live",
                                            Cards = new[]
                                                {
                                                    new LeankitBoardCard
                                                        {
                                                            Id = 123456
                                                        }
                                                }

                                        }
                                }

                        }
                };
                
            
            var apiCaller = this;
            Assert.That(new AllTicketsForBoardFactory(apiCaller, new ActivityIsInProgressSpecification(), new TicketActivityFactory()).Build().Tickets.ElementAt(1).Id, Is.EqualTo(123456));
        }

        [Test]
        public void SetsTicketTitle()
        {
            _expectedTitle = "Test Ticket";
            var apiCaller = this;
            Assert.That(new AllTicketsForBoardFactory(apiCaller, new ActivityIsInProgressSpecification(), new TicketActivityFactory()).Build().Tickets.First().Title, Is.EqualTo(_expectedTitle));
        }

        [Test]
        public void SetsTicketStarted()
        {
            _cardHistory = new[] 
                {
                    new LeanKitCardHistory
                        {
                            Type = "CardCreationEventDTO",
                            DateTime = "15/02/2013 at 10:50:35 AM",
                            ToLaneTitle = "DEV WIP"
                        }
                };

            var apiCaller = this;
            Assert.That(new AllTicketsForBoardFactory(apiCaller, new ActivityIsInProgressSpecification(), new TicketActivityFactory()).Build().Tickets.First().Started, Is.EqualTo(new DateTime(2013, 02, 15, 10, 50, 35)));
        }

        [Test]
        public void SetsTicketStartedWhenInitialStateIsReadyForDev()
        {
            _cardHistory = new[] 
                {
                    new LeanKitCardHistory
                        {
                            Type = "CardCreationEventDTO",
                            DateTime = "14/02/2013 at 2:23:11 PM",
                            ToLaneTitle = "READY FOR DEV"
                        },
                    new LeanKitCardHistory
                        {
                            Type = "CardMoveEventDTO",
                            DateTime = "15/02/2013 at 10:50:35 AM",
                            ToLaneTitle = "DEV WIP"
                        }
                };

            var apiCaller = this;
            Assert.That(new AllTicketsForBoardFactory(apiCaller, new ActivityIsInProgressSpecification(), new TicketActivityFactory()).Build().Tickets.First().Started, Is.EqualTo(new DateTime(2013, 02, 15, 10, 50, 35)));
        }

        [Test]
        public void SetsTicketFirstActivityTitleSet()
        {
            _cardHistory = new[] 
                {
                    new LeanKitCardHistory
                        {
                            Type = "CardCreationEventDTO",
                            DateTime = "14/02/2013 at 2:23:11 PM",
                            ToLaneTitle = "Ready for DEV"
                        }
                };

            var apiCaller = this;
            Assert.That(new AllTicketsForBoardFactory(apiCaller, new ActivityIsInProgressSpecification(), new TicketActivityFactory()).Build().Tickets.First().Activities.First().Title, Is.EqualTo("Ready for DEV"));
        }

        [Test]
        public void SetsTicketFirstActivityStarted()
        {
            _cardHistory = new[] 
                {
                    new LeanKitCardHistory
                        {
                            Type = "CardCreationEventDTO",
                            DateTime = "14/02/2013 at 2:23:11 PM",
                            ToLaneTitle = "Ready for DEV"
                        }
                };

            var apiCaller = this;
            Assert.That(new AllTicketsForBoardFactory(apiCaller, new ActivityIsInProgressSpecification(), new TicketActivityFactory()).Build().Tickets.First().Activities.First().Started, Is.EqualTo(new DateTime(2013, 02, 14, 14, 23, 11)));
        }

        [Test]
        public void SetsTicketCurrentActivityFinishedSetToDateTimeMin()
        {
            _cardHistory = new[] 
                {
                    new LeanKitCardHistory
                        {
                            Type = "CardCreationEventDTO",
                            DateTime = "14/02/2013 at 2:23:11 PM",
                            ToLaneTitle = "Ready for DEV"
                        }
                };

            var apiCaller = this;
            Assert.That(new AllTicketsForBoardFactory(apiCaller, new ActivityIsInProgressSpecification(), new TicketActivityFactory()).Build().Tickets.First().Activities.First().Finished, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void SetsTicketActivityFinished()
        {
            _cardHistory = new[] 
                {
                    new LeanKitCardHistory
                        {
                            Type = "CardCreationEventDTO",
                            DateTime = "14/02/2013 at 2:23:11 PM",
                            ToLaneTitle = "READY FOR DEV"
                        },
                    new LeanKitCardHistory
                        {
                            Type = "CardMoveEventDTO",
                            DateTime = "15/02/2013 at 10:50:35 AM",
                            ToLaneTitle = "DEV WIP"
                        }
                };

            var apiCaller = this;
            Assert.That(new AllTicketsForBoardFactory(apiCaller, new ActivityIsInProgressSpecification(), new TicketActivityFactory()).Build().Tickets.First().Activities.First().Finished, Is.EqualTo(new DateTime(2013, 02, 15, 10, 50, 35)));
        }

        public LeankitBoard GetBoard()
        {
            return new LeankitBoard
                {
                    Lanes = new[]
                        {
                            new LeankitBoardLane
                                {
                                    Cards = new[]
                                        {
                                            new LeankitBoardCard
                                                {
                                                    Id = _expectedId,
                                                    Title = _expectedTitle
                                                }
                                        }
                                }
                        }
                };
        }

        public IEnumerable<LeanKitCardHistory> GetCardHistory(int cardId)
        {
            return _cardHistory;
        }

        public LeankitBoardLaneWrapper GetBoardArchive()
        {
            return _boardArchive;
        }
    }
}
