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

            Assert.That(ticketActivityFactory.Build(leanKitCardHistory, null).Title, Is.EqualTo("Ready for DEV"));
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

            Assert.That(ticketActivityFactory.Build(leanKitCardHistory, null).Title, Is.EqualTo("Blocked: Ready for DEV"));
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

            Assert.That(ticketActivityFactory.Build(leanKitCardHistory, null).Started, Is.EqualTo(new DateTime(2013, 02, 14, 14, 23, 11)));
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

            Assert.That(ticketActivityFactory.Build(leanKitCardHistory, null).Finished, Is.EqualTo(DateTime.MinValue));
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

            Assert.That(ticketActivityFactory.Build(currentHistoryItem, nextHistoryItem).Finished, Is.EqualTo(new DateTime(2013, 02, 15, 10, 50, 35)));
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

            Assert.That(ticketActivityFactory.Build(currentHistoryItem, nextHistoryItem).Duration, Is.EqualTo(expectedWorkDuration));
        }

        public WorkDuration CalculateDuration(DateTime start, DateTime end)
        {
            return _workDuration;
        }
    }
}