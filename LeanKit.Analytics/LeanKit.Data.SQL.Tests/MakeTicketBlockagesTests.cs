using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LeanKit.Data.SQL.Tests
{
    [TestFixture]
    public class MakeTicketBlockagesTests
    {
        [Test]
        public void SetsBlockageReason()
        {
            var ticketActivityRecords = new List<TicketBlockedRecord>
                {
                    new TicketBlockedRecord { Reason = "Reason" }
                };
            Assert.That(new MakeTicketBlockages().Build(ticketActivityRecords).First().Reason, Is.EqualTo("Reason"));
        }

        [Test]
        public void SetsBlockageStarted()
        {
            var ticketActivityRecords = new List<TicketBlockedRecord>
                {
                    new TicketBlockedRecord { Started = new DateTime(2013, 1, 1) }
                };
            Assert.That(new MakeTicketBlockages().Build(ticketActivityRecords).First().Started, Is.EqualTo(new DateTime(2013, 1, 1)));
        }

        [Test]
        public void SetsBlockageFinished()
        {
            var ticketActivityRecords = new List<TicketBlockedRecord>
                {
                    new TicketBlockedRecord { Finished = new DateTime(2013, 1, 1) }
                };
            Assert.That(new MakeTicketBlockages().Build(ticketActivityRecords).First().Finished, Is.EqualTo(new DateTime(2013, 1, 1)));
        }
    }
}