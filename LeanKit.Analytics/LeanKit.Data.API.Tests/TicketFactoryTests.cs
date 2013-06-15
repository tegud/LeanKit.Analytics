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
    public class TicketFactoryTests : ITicketActivitiesFactory, ICalculateWorkDuration
    {
        private WorkDuration _workDuration;
        private List<TicketActivity> _ticketActivities = new List<TicketActivity>(0);

        [Test]
        public void SetsId()
        {
            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory);

            Assert.That(ticketFactory.Build(new LeankitBoardCard { Id = 12345, AssignedUsers = new LeanKitAssignedUser[0] }).Id, Is.EqualTo(12345));
        }

        [Test]
        public void SetsTitle()
        {
            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory);

            Assert.That(ticketFactory.Build(new LeankitBoardCard { Title = "Test Title", AssignedUsers = new LeanKitAssignedUser[0] }).Title, Is.EqualTo("Test Title"));
        }

        [Test]
        public void SetsExternalId()
        {
            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory);

            Assert.That(ticketFactory.Build(new LeankitBoardCard { ExternalCardID = "R-12345", AssignedUsers = new LeanKitAssignedUser[0] }).ExternalId, Is.EqualTo("R-12345"));
        }

        [Test]
        public void SetsSize()
        {
            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory);

            Assert.That(ticketFactory.Build(new LeankitBoardCard { Size = 1, AssignedUsers = new LeanKitAssignedUser[0] }).Size, Is.EqualTo(1));
        }

        [Test]
        public void SetsStarted()
        {
            var expectedStartDate = new DateTime(2013, 02, 15, 10, 50, 35);

            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, new FakeMileStoneFactory(expectedStartDate), fakeMileStoneFactory);

            Assert.That(ticketFactory.Build(new LeankitBoardCard { AssignedUsers = new LeanKitAssignedUser[0] }).Started, Is.EqualTo(expectedStartDate));
        }

        [Test]
        public void SetsFinished()
        {
            var expectedFinished = new DateTime(2013, 02, 16, 10, 50, 35);

            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, new FakeMileStoneFactory(expectedFinished));

            Assert.That(ticketFactory.Build(new LeankitBoardCard { AssignedUsers = new LeanKitAssignedUser[0] }).Finished, Is.EqualTo(expectedFinished));
        }

        [Test]
        public void SetsCycleTime()
        {
            var expectedDuration = new WorkDuration();

            _workDuration = expectedDuration;

            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory);

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

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory);

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

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory);

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

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory);

            Assert.That(ticketFactory.Build(new LeankitBoardCard
            {
                AssignedUsers = new[]
                        {
                            new LeanKitAssignedUser { AssignedUserId = assignedUserId }
                        }
            }).AssignedUsers.First().Email.Address, Is.EqualTo(expectedEmail));
        }

        public IEnumerable<TicketActivity> Build(LeankitBoardCard card)
        {
            
            return _ticketActivities;
        }

        public WorkDuration CalculateDuration(DateTime start, DateTime end)
        {
            return _workDuration;
        }
    }
}