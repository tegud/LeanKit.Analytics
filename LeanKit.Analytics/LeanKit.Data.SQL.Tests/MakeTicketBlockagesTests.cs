using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LeanKit.Data.SQL.Tests
{
    [TestFixture]
    public class MakeTicketBlockagesTests : ICalculateWorkDuration
    {
        private WorkDuration _durationOfBlockage;

        [Test]
        public void SetsBlockageReason()
        {
            var ticketActivityRecords = new List<TicketBlockedRecord>
                {
                    new TicketBlockedRecord { Reason = "Reason" }
                };

            ICalculateWorkDuration workDurationFactory = this;

            Assert.That(new TicketBlockageFactory(workDurationFactory).Build(ticketActivityRecords).First().Reason, Is.EqualTo("Reason"));
        }

        [Test]
        public void SetsBlockageStarted()
        {
            var ticketActivityRecords = new List<TicketBlockedRecord>
                {
                    new TicketBlockedRecord { Started = new DateTime(2013, 1, 1) }
                };

            ICalculateWorkDuration workDurationFactory = this;

            Assert.That(new TicketBlockageFactory(workDurationFactory).Build(ticketActivityRecords).First().Started, Is.EqualTo(new DateTime(2013, 1, 1)));
        }

        [Test]
        public void SetsBlockageFinished()
        {
            var ticketActivityRecords = new List<TicketBlockedRecord>
                {
                    new TicketBlockedRecord { Finished = new DateTime(2013, 1, 1) }
                };

            ICalculateWorkDuration workDurationFactory = this;

            Assert.That(new TicketBlockageFactory(workDurationFactory).Build(ticketActivityRecords).First().Finished, Is.EqualTo(new DateTime(2013, 1, 1)));
        }

        [Test]
        public void SetsBlockageDuration()
        {
            var expectedDuration = new WorkDuration();

            _durationOfBlockage = expectedDuration;

            var ticketActivityRecords = new List<TicketBlockedRecord>
                {
                    new TicketBlockedRecord()
                };

            ICalculateWorkDuration workDurationFactory = this;

            Assert.That(new TicketBlockageFactory(workDurationFactory).Build(ticketActivityRecords).First().Duration, Is.EqualTo(expectedDuration));
        }

        public WorkDuration CalculateDuration(DateTime start, DateTime end)
        {
            return _durationOfBlockage;
        }
    }
}