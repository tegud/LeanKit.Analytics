using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.APIClient.API;
using LeanKit.Data.API;
using NUnit.Framework;

namespace LeanKit.Data.Tests
{
    [TestFixture]
    public class AllTicketsForBoardFactoryTests : IApiCaller, ICalculateWorkDuration
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
                Lane = new LeankitBoardLane { Cards = new LeankitBoardCard[0] },
                ChildLanes = new LeankitBoardLaneWrapper[] {}
            };

        [Test]
        public void SetsTicketId()
        {
            _expectedId = 1234;
            var apiCaller = this;
            var workDurationFactory = this;
            var activityIsInProgressSpecification = new ActivityIsInProgressSpecification();
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);
            var ticketActivitiesFactory = new TicketActivitiesFactory(apiCaller, ticketActivityFactory);
            IActivitySpecification activityIsLiveSpecification = new ActivityIsLiveSpecification();
            var ticketFactory = new TicketFactory(ticketActivitiesFactory, new TicketCycleTimeDurationFactory(workDurationFactory), new TicketStartDateFactory(activityIsInProgressSpecification), new TicketFinishDateFactory(activityIsLiveSpecification));

            var allTicketsForBoardFactory = new AllBoardTicketsFromApi(apiCaller, ticketFactory);

            Assert.That(allTicketsForBoardFactory.Get().Tickets.First().Id, Is.EqualTo(_expectedId));
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
                                                            Title = "Example Card",
                                                            Id = 123456
                                                        }
                                                }

                                        }
                                }

                        }
            };


            var apiCaller = this;
            var workDurationFactory = this;
            var ticketCycleTimeDurationFactory = this;

            var activityIsInProgressSpecification = new ActivityIsInProgressSpecification();
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);
            var ticketActivitiesFactory = new TicketActivitiesFactory(apiCaller, ticketActivityFactory);
            IActivitySpecification activityIsLiveSpecification = new ActivityIsLiveSpecification();
            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, new TicketStartDateFactory(activityIsInProgressSpecification), new TicketFinishDateFactory(activityIsLiveSpecification));

            var allTicketsForBoardFactory = new AllBoardTicketsFromApi(apiCaller, ticketFactory);

            Assert.That(allTicketsForBoardFactory.Get().Tickets.ElementAt(1).Id, Is.EqualTo(123456));
        }

        [Test]
        public void ArchiveTicketsCardsOlderThanAreIgnored()
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
                                                            Id = 123456,
                                                            Title = "Cards older than"
                                                        }
                                                }

                                        }
                                }

                        }
            };


            var apiCaller = this;
            var workDurationFactory = this;
            var ticketCycleTimeDurationFactory = this;

            var activityIsInProgressSpecification = new ActivityIsInProgressSpecification();
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);
            var ticketActivitiesFactory = new TicketActivitiesFactory(apiCaller, ticketActivityFactory);
            IActivitySpecification activityIsLiveSpecification = new ActivityIsLiveSpecification();
            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, new TicketStartDateFactory(activityIsInProgressSpecification), new TicketFinishDateFactory(activityIsLiveSpecification));

            var allTicketsForBoardFactory = new AllBoardTicketsFromApi(apiCaller, ticketFactory);

            Assert.That(allTicketsForBoardFactory.Get().Tickets.Count(), Is.EqualTo(1));
        }

        [Test]
        public void SetsTicketCycleTime()
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
                        },
                    new LeanKitCardHistory
                        {
                            Type = "CardMoveEventDTO",
                            DateTime = "17/02/2013 at 3:50:35 PM",
                            ToLaneTitle = "LIVE"
                        }
                };

            var apiCaller = this;
            var workDurationFactory = this;
            var ticketCycleTimeDurationFactory = this;

            var activityIsInProgressSpecification = new ActivityIsInProgressSpecification();
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);
            var ticketActivitiesFactory = new TicketActivitiesFactory(apiCaller, ticketActivityFactory);

            IActivitySpecification activityIsLiveSpecification = new ActivityIsLiveSpecification();
            var ticketFactory = new TicketFactory(ticketActivitiesFactory, 
                ticketCycleTimeDurationFactory, new TicketStartDateFactory(activityIsInProgressSpecification), new TicketFinishDateFactory(activityIsLiveSpecification));

            var allTicketsForBoardFactory = new AllBoardTicketsFromApi(apiCaller, ticketFactory);

            Assert.That(allTicketsForBoardFactory.Get().Tickets.First().CycleTime.Days, Is.EqualTo(2));
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

        public WorkDuration CalculateDuration(DateTime start, DateTime end)
        {
            return new WorkDuration
                {
                    Days = 2,
                    Hours = 12
                };
        }
    }
}
