using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LeanKit.Data.SQL.Tests
{
    [TestFixture]
    public class TicketFactoryTests : ICalculateWorkDuration
    {
        private WorkDuration _workDuration;

        [Test]
        public void SetsTicketTitle()
        {
            const string expectedTitle = "Test Title";

            Assert.That(new TicketFactory(this).Build(new TicketRecord
            {
                Title = expectedTitle
            }).Title, Is.EqualTo(expectedTitle));
        }

        [Test]
        public void SetsTicketId()
        {
            const int expectedId = 12345;

            Assert.That(new TicketFactory(this).Build(new TicketRecord
            {
                Id = expectedId
            }).Id, Is.EqualTo(expectedId));
        }

        [Test]
        public void SetsTicketActivityTitle()
        {
            const string expectedActivityTitle = "Dev WIP";

            Assert.That(new TicketFactory(this).Build(new TicketRecord
            {
                Activities = new List<TicketActivityRecord>
                    {
                        new TicketActivityRecord
                            {
                                Title = "Dev WIP"
                            }
                    }
            }).Activities.First().Title, Is.EqualTo(expectedActivityTitle));
        }

        [Test]
        public void SetsTicketActivityFinishedWhenItIsTheCurrentActivity()
        {
            Assert.That(new TicketFactory(this).Build(new TicketRecord
            {
                Activities = new List<TicketActivityRecord>
                    {
                        new TicketActivityRecord
                            {
                                Date = new DateTime(2013, 05, 01)
                            }
                    }
            }).Activities.First().Finished, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void SetsTicketActivityDuration()
        {
            var expectedDuration = new WorkDuration();

            _workDuration = expectedDuration;

            ICalculateWorkDuration workDurationFactory = this;

            Assert.That(new TicketFactory(workDurationFactory).Build(new TicketRecord
            {
                Activities = new List<TicketActivityRecord>
                    {
                        new TicketActivityRecord
                            {
                                Date = new DateTime(2013, 05, 01)
                            },
                        new TicketActivityRecord
                            {
                                Date = new DateTime(2013, 05, 01)
                            }
                    }
            }).Activities.First().Duration, Is.EqualTo(expectedDuration));
        }

        public WorkDuration CalculateDuration(DateTime start, DateTime end)
        {
            return _workDuration;
        }
    }
}
