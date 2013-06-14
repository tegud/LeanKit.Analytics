using System;
using NUnit.Framework;

namespace LeanKit.Data.SQL.Tests
{
    [TestFixture]
    public class TicketActivityFactoryTests : ICalculateWorkDuration
    {
        private WorkDuration _workDuration;

        [Test]
        public void SetsTicketActivityTitle()
        {
            const string expectedActivityTitle = "Dev WIP";

            var current = new TicketActivityRecord { Activity = expectedActivityTitle };

            Assert.That(new TicketActivityFactory(this).Build(current, null).Title, Is.EqualTo(expectedActivityTitle));
        }

        [Test]
        public void SetsTicketActivityStartedToCurrentActivityDate()
        {
            var expectedActivityStarted = new DateTime();

            var current = new TicketActivityRecord { Date = expectedActivityStarted };

            Assert.That(new TicketActivityFactory(this).Build(current, null).Started, Is.EqualTo(expectedActivityStarted));
        }

        [Test]
        public void SetsTicketActivityFinishedToNextActivityDate()
        {
            var expectedActivityFinished = new DateTime(2013, 5, 3);

            var current = new TicketActivityRecord { Date = new DateTime() };
            var next = new TicketActivityRecord { Date = expectedActivityFinished };

            Assert.That(new TicketActivityFactory(this).Build(current, next).Finished, Is.EqualTo(expectedActivityFinished));
        }

        [Test]
        public void SetsTicketActivityFinishedWhenItIsTheCurrentActivity()
        {
            var expectedActivityFinished = new DateTime();

            var current = new TicketActivityRecord { Date = new DateTime(2013, 1, 1) };

            Assert.That(new TicketActivityFactory(this).Build(current, null).Finished, Is.EqualTo(expectedActivityFinished));
        }

        [Test]
        public void SetsTicketActivityDuration()
        {
            var expectedDuration = new WorkDuration();

            _workDuration = expectedDuration;

            ICalculateWorkDuration workDurationFactory = this;

            var current = new TicketActivityRecord
                {
                    Date = new DateTime(2013, 05, 01)
                };
            var next = new TicketActivityRecord
                {
                    Date = new DateTime(2013, 05, 01)
                };

            Assert.That(new TicketActivityFactory(workDurationFactory).Build(current, next).Duration, Is.EqualTo(expectedDuration));
        }

        [Test]
        public void SetsAssignedUserToUnAssignedWhenUserIdIsZero()
        {
            var current = new TicketActivityRecord { AssignedUserId = 0 };

            Assert.That(new TicketActivityFactory(this).Build(current, null).AssignedUser, Is.EqualTo(TicketActivityAssignedUser.UnAssigned));
        }

        [Test]
        public void SetsAssignedUserId()
        {
            var current = new TicketActivityRecord { AssignedUserId = 12345, AssignedUserEmail = "mr.developer@example.com" };

            Assert.That(new TicketActivityFactory(this).Build(current, null).AssignedUser.Id, Is.EqualTo(12345));
        }

        [Test]
        public void SetsAssignedUserName()
        {
            var current = new TicketActivityRecord { AssignedUserId = 12345, AssignedUserName = "A Developer", AssignedUserEmail = "mr.developer@example.com" };

            Assert.That(new TicketActivityFactory(this).Build(current, null).AssignedUser.Name, Is.EqualTo("A Developer"));
        }

        [Test]
        public void SetsAssignedUserEmail()
        {
            var current = new TicketActivityRecord { AssignedUserId = 12345, AssignedUserEmail = "mr.developer@example.com" };

            Assert.That(new TicketActivityFactory(this).Build(current, null).AssignedUser.Email.Address, Is.EqualTo("mr.developer@example.com"));
        }

        public WorkDuration CalculateDuration(DateTime start, DateTime end)
        {
            return _workDuration;
        }

        public TicketActivity BuildActivity(TicketActivityRecord current, TicketActivityRecord next)
        {
            throw new NotImplementedException();
        }
    }
}