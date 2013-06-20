using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using LeanKit.APIClient.API;
using LeanKit.Data.Tests;
using NUnit.Framework;

namespace LeanKit.Data.API.Tests
{
    [TestFixture]
    public class TicketBlockagesFactoryTests
    {
        [Test]
        public void SetsBlockageStarted()
        {
            var expectedStarted = new DateTime(2013, 02, 14, 14, 23, 11);
            var leanKitCardHistories = new[]
                {
                    new LeanKitCardHistory
                        {
                            Type = "CardBlockedEventDTO",
                            IsBlocked = true,
                            DateTime = "14/02/2013 at 2:23:11 PM", 
                        }
                };

            Assert.That(new TicketBlockagesFactory().Build(leanKitCardHistories).First().Started, Is.EqualTo(expectedStarted));
        }

        [Test]
        public void SetsBlockageFinishedToNextItemsStarted()
        {
            var expectedFinished = new DateTime(2013, 02, 14, 18, 23, 11);
            var leanKitCardHistories = new[]
                {
                    new LeanKitCardHistory
                        {
                            Type = "CardBlockedEventDTO",
                            IsBlocked = true,
                            DateTime = "14/02/2013 at 2:23:11 PM", 
                        },
                    new LeanKitCardHistory
                        {
                            Type = "CardBlockedEventDTO",
                            DateTime = "14/02/2013 at 6:23:11 PM", 
                        }
                };

            Assert.That(new TicketBlockagesFactory().Build(leanKitCardHistories).First().Finished, Is.EqualTo(expectedFinished));
        }

        [Test]
        public void SetsFinishedToMinValueWhenBlockageIsOngoing()
        {
            var leanKitCardHistories = new[]
                {
                    new LeanKitCardHistory
                        {
                            Type = "CardBlockedEventDTO",
                            IsBlocked = true,
                            DateTime = "14/02/2013 at 2:23:11 PM", 
                        }
                };

            Assert.That(new TicketBlockagesFactory().Build(leanKitCardHistories).First().Finished, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void SetsBlockedReason()
        {
            var leanKitCardHistories = new[]
                {
                    new LeanKitCardHistory
                        {
                            Type = "CardBlockedEventDTO",
                            DateTime = "14/02/2013 at 2:23:11 PM",
                            IsBlocked = true,
                            Comment = "Blocked Reason"
                        }
                };

            Assert.That(new TicketBlockagesFactory().Build(leanKitCardHistories).First().Reason, Is.EqualTo("Blocked Reason"));
        }

        [Test]
        public void BlockEndingEventsAreNotIncluded()
        {
            var leanKitCardHistories = new[]
                {
                    new LeanKitCardHistory
                        {
                            Type = "CardBlockedEventDTO",
                            DateTime = "14/02/2013 at 2:23:11 PM",
                            IsBlocked = true,
                            Comment = "Blocked Reason"
                        },
                    new LeanKitCardHistory
                        {
                            Type = "CardBlockedEventDTO",
                            DateTime = "14/02/2013 at 6:23:11 PM",
                            IsBlocked = false
                        }
                };

            Assert.That(new TicketBlockagesFactory().Build(leanKitCardHistories).Count(), Is.EqualTo(1));
        }
    }

    [TestFixture]
    public class TicketFactoryTests : ITicketActivitiesFactory, ICalculateWorkDuration, IMakeTicketBlockages, IApiCaller
    {
        private WorkDuration _workDuration;
        private List<TicketActivity> _ticketActivities = new List<TicketActivity>(0);
        private IEnumerable<TicketBlockage> _blockages = new TicketBlockage[0];
        private IEnumerable<LeanKitCardHistory> _cardHistory = new LeanKitCardHistory[0];
        private IEnumerable<LeanKitCardHistory> _cardHistoryPassedToBlockagesFactory = new LeanKitCardHistory[0];
        private IEnumerable<LeanKitCardHistory> _cardHistoryPassedToActivitiesFactory = new LeanKitCardHistory[0];

        [Test]
        public void SetsId()
        {
            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;
            IMakeTicketBlockages ticketBlockagesFactory = this;
            IApiCaller apiCaller = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory, ticketBlockagesFactory, apiCaller);

            Assert.That(ticketFactory.Build(new LeankitBoardCard { Id = 12345, AssignedUsers = new LeanKitAssignedUser[0] }).Id, Is.EqualTo(12345));
        }

        [Test]
        public void SetsTitle()
        {
            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;
            IMakeTicketBlockages ticketBlockagesFactory = this;
            IApiCaller apiCaller = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory, ticketBlockagesFactory, apiCaller);

            Assert.That(ticketFactory.Build(new LeankitBoardCard { Title = "Test Title", AssignedUsers = new LeanKitAssignedUser[0] }).Title, Is.EqualTo("Test Title"));
        }

        [Test]
        public void SetsExternalId()
        {
            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;
            IMakeTicketBlockages ticketBlockagesFactory = this;
            IApiCaller apiCaller = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory, ticketBlockagesFactory, apiCaller);

            Assert.That(ticketFactory.Build(new LeankitBoardCard { ExternalCardID = "R-12345", AssignedUsers = new LeanKitAssignedUser[0] }).ExternalId, Is.EqualTo("R-12345"));
        }

        [Test]
        public void SetsSize()
        {
            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;
            IMakeTicketBlockages ticketBlockagesFactory = this;
            IApiCaller apiCaller = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory, ticketBlockagesFactory, apiCaller);

            Assert.That(ticketFactory.Build(new LeankitBoardCard { Size = 1, AssignedUsers = new LeanKitAssignedUser[0] }).Size, Is.EqualTo(1));
        }

        [Test]
        public void SetsStarted()
        {
            var expectedStartDate = new DateTime(2013, 02, 15, 10, 50, 35);

            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;
            IMakeTicketBlockages ticketBlockagesFactory = this;
            IApiCaller apiCaller = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, new FakeMileStoneFactory(expectedStartDate), fakeMileStoneFactory, ticketBlockagesFactory, apiCaller);

            Assert.That(ticketFactory.Build(new LeankitBoardCard { AssignedUsers = new LeanKitAssignedUser[0] }).Started, Is.EqualTo(expectedStartDate));
        }

        [Test]
        public void SetsFinished()
        {
            var expectedFinished = new DateTime(2013, 02, 16, 10, 50, 35);

            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;
            IMakeTicketBlockages ticketBlockagesFactory = this;
            IApiCaller apiCaller = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, new FakeMileStoneFactory(expectedFinished), ticketBlockagesFactory, apiCaller);

            Assert.That(ticketFactory.Build(new LeankitBoardCard { AssignedUsers = new LeanKitAssignedUser[0] }).Finished, Is.EqualTo(expectedFinished));
        }

        [Test]
        public void SetsCycleTime()
        {
            var expectedDuration = new WorkDuration();

            _workDuration = expectedDuration;

            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;
            IMakeTicketBlockages ticketBlockagesFactory = this;
            IApiCaller apiCaller = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory, ticketBlockagesFactory, apiCaller);

            Assert.That(ticketFactory.Build(new LeankitBoardCard { AssignedUsers = new LeanKitAssignedUser[0]  }).CycleTime, Is.EqualTo(expectedDuration));
        }

        [Test]
        public void SetsAssignedUserId()
        {
            const int expectedId = 12345;

            _ticketActivities = new List<TicketActivity>
                {
                    new TicketActivity { AssignedUser = new TicketAssignedUser
                        {
                            Id = expectedId,
                        }}
                };

            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;
            IMakeTicketBlockages ticketBlockagesFactory = this;
            IApiCaller apiCaller = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory, ticketBlockagesFactory, apiCaller);

            Assert.That(ticketFactory.Build(new LeankitBoardCard { AssignedUsers = new[] { new LeanKitAssignedUser { AssignedUserId = 12345 } } }).AssignedUsers.First().Id, Is.EqualTo(expectedId));
        }

        [Test]
        public void SetsAssignedUserName()
        {
            const string expectedName = "Mr Developer";
            const int assignedUserId = 12345;

            _ticketActivities = new List<TicketActivity>
                {
                    new TicketActivity { AssignedUser = new TicketAssignedUser
                        {
                            Id = assignedUserId,
                            Name = expectedName
                        }}
                };

            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;
            IMakeTicketBlockages ticketBlockagesFactory = this;
            IApiCaller apiCaller = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory, ticketBlockagesFactory, apiCaller);

            Assert.That(ticketFactory.Build(new LeankitBoardCard
            {
                AssignedUsers = new[]
                        {
                            new LeanKitAssignedUser { AssignedUserId = assignedUserId }
                        }
            }).AssignedUsers.First().Name, Is.EqualTo(expectedName));
        }

        [Test]
        public void SetsAssignedUserEmail()
        {
            const string expectedEmail = "dev@example.com";
            const int assignedUserId = 12345;

            _ticketActivities = new List<TicketActivity>
                {
                    new TicketActivity { AssignedUser = new TicketAssignedUser
                        {
                            Id = assignedUserId,
                            Email = new MailAddress(expectedEmail)
                        }}
                };

            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;
            IMakeTicketBlockages ticketBlockagesFactory = this;
            IApiCaller apiCaller = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory, ticketBlockagesFactory, apiCaller);

            Assert.That(ticketFactory.Build(new LeankitBoardCard
            {
                AssignedUsers = new[]
                        {
                            new LeanKitAssignedUser { AssignedUserId = assignedUserId }
                        }
            }).AssignedUsers.First().Email.Address, Is.EqualTo(expectedEmail));
        }

        [Test]
        public void SetsBlockages()
        {
            _blockages = new List<TicketBlockage>(0);

            var leankitBoardCard = new LeankitBoardCard { AssignedUsers = new LeanKitAssignedUser[0] };

            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;
            IMakeTicketBlockages ticketBlockagesFactory = this;
            IApiCaller apiCaller = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory, ticketBlockagesFactory, apiCaller);

            Assert.That(ticketFactory.Build(leankitBoardCard).Blockages, Is.EqualTo(_blockages));
        }

        [Test]
        public void CardHistoryFromApiPassedToBlockagesFactory()
        {
            _blockages = new List<TicketBlockage>(0);
            _cardHistory = new LeanKitCardHistory[0];

            var leankitBoardCard = new LeankitBoardCard { AssignedUsers = new LeanKitAssignedUser[0] };

            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;
            IMakeTicketBlockages ticketBlockagesFactory = this;
            IApiCaller apiCaller = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory, ticketBlockagesFactory, apiCaller).Build(leankitBoardCard);

            Assert.That(_cardHistoryPassedToBlockagesFactory, Is.EqualTo(_cardHistory));
        }

        [Test]
        public void CardHistoryFromApiPassedToActivitiesFactory()
        {
            _blockages = new List<TicketBlockage>(0);
            _cardHistory = new LeanKitCardHistory[0];

            var leankitBoardCard = new LeankitBoardCard { AssignedUsers = new LeanKitAssignedUser[0] };

            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;
            IMakeTicketBlockages ticketBlockagesFactory = this;
            IApiCaller apiCaller = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory, ticketBlockagesFactory, apiCaller).Build(leankitBoardCard);

            Assert.That(_cardHistoryPassedToActivitiesFactory, Is.EqualTo(_cardHistory));
        }

        public WorkDuration CalculateDuration(DateTime start, DateTime end)
        {
            return _workDuration;
        }

        public LeankitBoard GetBoard()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LeanKitCardHistory> GetCardHistory(int cardId)
        {
            return _cardHistory;
        }

        public LeankitBoardLaneWrapper GetBoardArchive()
        {
            throw new NotImplementedException();
        }

        IEnumerable<TicketActivity> ITicketActivitiesFactory.Build(IEnumerable<LeanKitCardHistory> cardHistory)
        {
            _cardHistoryPassedToActivitiesFactory = cardHistory;
            return _ticketActivities;
        }

        IEnumerable<TicketBlockage> IMakeTicketBlockages.Build(IEnumerable<LeanKitCardHistory> cardHistory)
        {
            _cardHistoryPassedToBlockagesFactory = cardHistory;
            return _blockages;
        }
    }
}