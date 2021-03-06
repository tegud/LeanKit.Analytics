using System;
using LeanKit.APIClient.API;
using NUnit.Framework;

namespace LeanKit.Data.API.Tests
{
    [TestFixture]
    public class TicketActivityFactoryTests : ICalculateWorkDuration
    {
        private WorkDuration _workDuration;

        [Test]
        public void SetsTitle()
        {
            var leanKitCardHistory = new LeanKitCardHistory
                {
                    Type = "CardCreationEventDTO", 
                    DateTime = "14/02/2013 at 2:23:11 PM", 
                    ToLaneTitle = "Ready for DEV"
                };

            var workDurationFactory = this;
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);

            Assert.That(ticketActivityFactory.Build(leanKitCardHistory, null, null).Title, Is.EqualTo("Ready for DEV"));
        }

        [Test]
        public void SetsTicketFirstBlockedActivityTitleSet()
        {
            var leanKitCardHistory = new LeanKitCardHistory
                {
                    Type = "CardBlockedEventDTO", 
                    IsBlocked = true, 
                    DateTime = "14/02/2013 at 2:23:11 PM", 
                    ToLaneTitle = "Ready for DEV"
                };

            var workDurationFactory = this;
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);

            Assert.That(ticketActivityFactory.Build(leanKitCardHistory, null, null).Title, Is.EqualTo("Blocked: Ready for DEV"));
        }

        [Test]
        public void SetsTicketFirstActivityStarted()
        {
            var leanKitCardHistory = new LeanKitCardHistory
                {
                    Type = "CardCreationEventDTO", 
                    DateTime = "14/02/2013 at 2:23:11 PM", 
                    ToLaneTitle = "Ready for DEV"
                };

            var workDurationFactory = this;
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);

            Assert.That(ticketActivityFactory.Build(leanKitCardHistory, null, null).Started, Is.EqualTo(new DateTime(2013, 02, 14, 14, 23, 11)));
        }

        [Test]
        public void SetsTicketCurrentActivityFinishedSetToDateTimeMin()
        {
            var leanKitCardHistory = new LeanKitCardHistory
                {
                    Type = "CardCreationEventDTO", 
                    DateTime = "14/02/2013 at 2:23:11 PM", 
                    ToLaneTitle = "Ready for DEV"
                };

            var workDurationFactory = this;
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);

            Assert.That(ticketActivityFactory.Build(leanKitCardHistory, null, null).Finished, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void SetsTicketActivityFinished()
        {
            var currentHistoryItem = new LeanKitCardHistory
                {
                    Type = "CardCreationEventDTO", DateTime = "14/02/2013 at 2:23:11 PM", ToLaneTitle = "READY FOR DEV"
                };
            var nextHistoryItem = new LeanKitCardHistory
                {
                    Type = "CardMoveEventDTO", DateTime = "15/02/2013 at 10:50:35 AM", ToLaneTitle = "DEV WIP"
                };

            var workDurationFactory = this;
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);

            Assert.That(ticketActivityFactory.Build(currentHistoryItem, null, nextHistoryItem).Finished, Is.EqualTo(new DateTime(2013, 02, 15, 10, 50, 35)));
        }

        [Test]
        public void SetsTicketActivityDuration()
        {
            var expectedWorkDuration = new WorkDuration();

            _workDuration = expectedWorkDuration;

            var currentHistoryItem = new LeanKitCardHistory
                {
                    Type = "CardCreationEventDTO", DateTime = "14/02/2013 at 2:23:11 PM", ToLaneTitle = "READY FOR DEV"
                };
            var nextHistoryItem = new LeanKitCardHistory
                {
                    Type = "CardMoveEventDTO", DateTime = "15/02/2013 at 10:50:35 AM", ToLaneTitle = "DEV WIP"
                };

            var workDurationFactory = this;
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);

            Assert.That(ticketActivityFactory.Build(currentHistoryItem, null, nextHistoryItem).Duration, Is.EqualTo(expectedWorkDuration));
        }

        [Test]
        public void SetTicketActivityAssignedUserToUnAssignedWhenUserIdIsNotSet()
        {
            var currentHistoryItem = new LeanKitCardHistory
            {
                Type = "UserAssignmentEventDTO",
                DateTime = "14/02/2013 at 2:23:11 PM",
                ToLaneTitle = "READY FOR DEV"
            };

            var workDurationFactory = this;
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);

            Assert.That(ticketActivityFactory.Build(currentHistoryItem, null, null).AssignedUser, Is.EqualTo(TicketAssignedUser.UnAssigned));
        }

        [Test]
        public void SetTicketActivityAssignedUserName()
        {
            var currentHistoryItem = new LeanKitCardHistory
            {
                Type = "UserAssignmentEventDTO",
                DateTime = "14/02/2013 at 2:23:11 PM",
                ToLaneTitle = "READY FOR DEV",
                AssignedUserId = 1,
                AssignedUserFullName = "Mr Developer",
                AssignedUserEmailAddres = "developer@example.com"
            };

            var workDurationFactory = this;
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);

            Assert.That(ticketActivityFactory.Build(currentHistoryItem, null, null).AssignedUser.Name, Is.EqualTo("Mr Developer"));
        }

        [Test]
        public void SetTicketActivityAssignedUserId()
        {
            var currentHistoryItem = new LeanKitCardHistory
            {
                Type = "UserAssignmentEventDTO",
                DateTime = "14/02/2013 at 2:23:11 PM",
                ToLaneTitle = "READY FOR DEV",
                AssignedUserId = 123423,
                AssignedUserEmailAddres = "developer@example.com"
            };

            var workDurationFactory = this;
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);

            Assert.That(ticketActivityFactory.Build(currentHistoryItem, null, null).AssignedUser.Id, Is.EqualTo(123423));
        }

        [Test]
        public void SetTicketActivityAssignedUserEmail()
        {
            var currentHistoryItem = new LeanKitCardHistory
            {
                Type = "UserAssignmentEventDTO",
                DateTime = "14/02/2013 at 2:23:11 PM",
                ToLaneTitle = "READY FOR DEV",
                AssignedUserId = 1,
                AssignedUserEmailAddres = "developer@example.com"
            };

            var workDurationFactory = this;
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);

            Assert.That(ticketActivityFactory.Build(currentHistoryItem, null, null).AssignedUser.Email.Address, Is.EqualTo("developer@example.com"));
        }

        [Test]
        public void SetTicketActivityAssignedUserToPreviousItemsUserIfCurrentIsUnassigned()
        {
            var currentHistoryItem = new LeanKitCardHistory
            {
                Type = "UserAssignmentEventDTO",
                DateTime = "14/02/2013 at 2:23:11 PM",
                ToLaneTitle = "READY FOR DEV"
            };
            var previousHistoryItem = new LeanKitCardHistory
            {
                Type = "UserAssignmentEventDTO",
                DateTime = "14/02/2013 at 2:23:11 PM",
                ToLaneTitle = "READY FOR DEV",
                AssignedUserId = 1,
                AssignedUserEmailAddres = "developer@example.com"
            };

            var workDurationFactory = this;
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);

            Assert.That(ticketActivityFactory.Build(currentHistoryItem, previousHistoryItem, null).AssignedUser.Email.Address, Is.EqualTo("developer@example.com"));
        }

        [Test]
        public void SetTicketActivityAssignedUserToUnassignedIfCurrentHistoryItemIsUnassignment()
        {
            var currentHistoryItem = new LeanKitCardHistory
            {
                Type = "UserAssignmentEventDTO",
                DateTime = "14/02/2013 at 2:23:11 PM",
                ToLaneTitle = "READY FOR DEV", 
                IsUnassigning = true
            };
            var previousHistoryItem = new LeanKitCardHistory
            {
                Type = "UserAssignmentEventDTO",
                DateTime = "14/02/2013 at 2:23:11 PM",
                ToLaneTitle = "READY FOR DEV",
                AssignedUserId = 1,
                AssignedUserEmailAddres = "developer@example.com"
            };

            var workDurationFactory = this;
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);

            Assert.That(ticketActivityFactory.Build(currentHistoryItem, previousHistoryItem, null).AssignedUser, Is.EqualTo(TicketAssignedUser.UnAssigned));
        }

        public WorkDuration CalculateDuration(DateTime start, DateTime end)
        {
            return _workDuration;
        }
    }
}