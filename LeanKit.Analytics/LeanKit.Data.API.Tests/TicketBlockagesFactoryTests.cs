using System;
using System.Linq;
using LeanKit.APIClient.API;
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
}