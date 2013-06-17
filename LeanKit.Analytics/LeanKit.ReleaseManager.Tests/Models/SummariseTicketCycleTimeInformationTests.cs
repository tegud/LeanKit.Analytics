using System.Linq;
using LeanKit.Data;
using LeanKit.ReleaseManager.Models;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class SummariseTicketCycleTimeInformationTests
    {

        [Test]
        public void NoTicketsSetsEmptySummary()
        {
            var tickets = new Ticket[0];

            Assert.That(new SummariseTicketCycleTimeInformation().Summarise(tickets).TicketCount, Is.EqualTo(0));
        }

        [Test]
        public void SetsNumberOfTickets()
        {
            var tickets = new[]
                {
                    new Ticket { CycleTime = new WorkDuration() }, 
                    new Ticket { CycleTime = new WorkDuration() }, 
                    new Ticket { CycleTime = new WorkDuration() }, 
                    new Ticket { CycleTime = new WorkDuration() }
                };

            Assert.That(new SummariseTicketCycleTimeInformation().Summarise(tickets).TicketCount, Is.EqualTo(4));
        }

        [Test]
        public void SetsAverageCycleTimeInDays()
        {
            var tickets = new[]
                {
                    new Ticket { CycleTime = new WorkDuration { Days = 1 } }, 
                    new Ticket { CycleTime = new WorkDuration { Days = 5 } }, 
                    new Ticket { CycleTime = new WorkDuration { Days = 3 } }
                };

            Assert.That(new SummariseTicketCycleTimeInformation().Summarise(tickets).AverageCycleTime, Is.EqualTo(3));
        }

        [Test]
        public void RoundsAverageCycleTimeDownWhenAppropriate()
        {
            var tickets = new[]
                {
                    new Ticket { CycleTime = new WorkDuration { Days = 1 } }, 
                    new Ticket { CycleTime = new WorkDuration { Days = 5 } }, 
                    new Ticket { CycleTime = new WorkDuration { Days = 4 } }
                };

            Assert.That(new SummariseTicketCycleTimeInformation().Summarise(tickets).AverageCycleTime, Is.EqualTo(3));
        }

        [Test]
        public void RoundsAverageCycleTimeUpWhenAppropriate()
        {
            var tickets = new[]
                {
                    new Ticket { CycleTime = new WorkDuration { Days = 2 } }, 
                    new Ticket { CycleTime = new WorkDuration { Days = 5 } }, 
                    new Ticket { CycleTime = new WorkDuration { Days = 4 } }
                };

            Assert.That(new SummariseTicketCycleTimeInformation().Summarise(tickets).AverageCycleTime, Is.EqualTo(4));
        }

        [Test]
        public void SetsMaxCycleTimeInDays()
        {
            var tickets = new[]
                {
                    new Ticket { CycleTime = new WorkDuration { Days = 1 } }, 
                    new Ticket { CycleTime = new WorkDuration { Days = 5 } }, 
                    new Ticket { CycleTime = new WorkDuration { Days = 3 } }
                };

            Assert.That(new SummariseTicketCycleTimeInformation().Summarise(tickets).MaximumCycleTime, Is.EqualTo(5));
        }

        [Test]
        public void SetsNumberOfTicketsWithNoEstimate()
        {
            var tickets = new[]
                {
                    new Ticket {CycleTime = new WorkDuration(), Size = 0 },
                    new Ticket {CycleTime = new WorkDuration(), Size = 1 },
                    new Ticket {CycleTime = new WorkDuration(), Size = 0 }
                };

            Assert.That(new SummariseTicketCycleTimeInformation().Summarise(tickets).NumberOfTicketsWithNoEstimate, Is.EqualTo(2));
        }

        [TestCase("?", 2)]
        [TestCase("1", 2)]
        [TestCase("2", 2)]
        public void SetsSizeAverageCycleTime(string size, int expectedAverageCycleTime)
        {
            var tickets = new[]
                {
                    new Ticket {CycleTime = new WorkDuration{ Days = 2 }, Size = 0 },
                    new Ticket {CycleTime = new WorkDuration{ Days = 2 }, Size = 0 },
                    new Ticket {CycleTime = new WorkDuration{ Days = 1 }, Size = 0 },
                    new Ticket {CycleTime = new WorkDuration{ Days = 2 }, Size = 1 },
                    new Ticket {CycleTime = new WorkDuration{ Days = 2 }, Size = 1 },
                    new Ticket {CycleTime = new WorkDuration{ Days = 3 }, Size = 1 },
                    new Ticket {CycleTime = new WorkDuration{ Days = 2 }, Size = 2 }
                };

            var ticketCycleTimeSummary = new SummariseTicketCycleTimeInformation().Summarise(tickets);
            Assert.That(ticketCycleTimeSummary.CycleTimeBySize.First(c => c.Size == size).CycleTime, Is.EqualTo(expectedAverageCycleTime));
        }

        [TestCase(0, "?")]
        [TestCase(1, "1")]
        [TestCase(2, "2")]
        public void SetsSizeOrder(int index, string expectedSize)
        {
            var tickets = new[]
                {
                    new Ticket {CycleTime = new WorkDuration{ Days = 2 }, Size = 1 },
                    new Ticket {CycleTime = new WorkDuration{ Days = 2 }, Size = 2 },
                    new Ticket {CycleTime = new WorkDuration{ Days = 2 }, Size = 1 },
                    new Ticket {CycleTime = new WorkDuration{ Days = 2 }, Size = 0 },
                    new Ticket {CycleTime = new WorkDuration{ Days = 1 }, Size = 0 },
                    new Ticket {CycleTime = new WorkDuration{ Days = 3 }, Size = 1 },
                    new Ticket {CycleTime = new WorkDuration{ Days = 2 }, Size = 0 }
                };

            var ticketCycleTimeSummary = new SummariseTicketCycleTimeInformation().Summarise(tickets);
            Assert.That(ticketCycleTimeSummary.CycleTimeBySize.First(c => c.Size == expectedSize).Size, Is.EqualTo(expectedSize));
        }
    }
}