using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;
using LeanKit.ReleaseManager.Models.CycleTime;
using NUnit.Framework;
using NUnit.Framework.Constraints;

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

    [TestFixture]
    public class CycleTimeViewModelFactoryTests : IGetReleasedTicketsFromTheDatabase, IMakeCycleTimeReleaseViewModels, IMakeTimePeriodViewModels, ISummariseTicketCycleTimeInformation
    {
        private Ticket _ticketReturnedFromDb;
        private CycleTimeQuery _queryPassedToRepository;
        private CycleTimeReleaseViewModel _cycleTimeTicketReleaseInfo;
        private CycleTimePeriodViewModel _cycleTimePeriodViewModel;
        private string _queryPassedToTimePeriodFactory;
        private TicketCycleTimeSummary _cycleTimeSummary;

        [Test]
        public void SetsTicketId()
        {
            const int expectedId = 12345;

            _ticketReturnedFromDb = new Ticket
                {
                    CycleTime = new WorkDuration(),
                    Id = expectedId
                };

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory, ticketSummaryFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new CycleTimeQuery());

            Assert.That(cycleTimeViewModel.Tickets.First().Id, Is.EqualTo(expectedId));
        }

        [Test]
        public void SetsTicketExternalId()
        {
            const string expectedExternalId = "R-1234";

            _ticketReturnedFromDb = new Ticket
            {
                CycleTime = new WorkDuration(),
                ExternalId = expectedExternalId
            };

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory, ticketSummaryFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new CycleTimeQuery());

            Assert.That(cycleTimeViewModel.Tickets.First().ExternalId, Is.EqualTo(expectedExternalId));
        }

        [Test]
        public void SetsTicketTitle()
        {
            const string expectedTitle = "Example Ticket";

            _ticketReturnedFromDb = new Ticket
            {
                CycleTime = new WorkDuration(),
                Title = expectedTitle
            };

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory, ticketSummaryFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new CycleTimeQuery());

            Assert.That(cycleTimeViewModel.Tickets.First().Title, Is.EqualTo(expectedTitle));
        }

        [Test]
        public void SetsTicketStartedFriendlyDate()
        {
            const string expectedStarted = "Today 10:00";

            _ticketReturnedFromDb = new Ticket
            {
                CycleTime = new WorkDuration(),
                Started = DateTime.Now.Date.AddHours(10)
            };

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory, ticketSummaryFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new CycleTimeQuery());

            Assert.That(cycleTimeViewModel.Tickets.First().StartedFriendlyText, Is.EqualTo(expectedStarted));
        }

        [Test]
        public void SetsTicketFinishedFriendlyDate()
        {
            const string expectedFinished = "Today 10:00";

            _ticketReturnedFromDb = new Ticket
            {
                CycleTime = new WorkDuration(),
                Finished = DateTime.Now.Date.AddHours(10)
            };

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory, ticketSummaryFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new CycleTimeQuery());

            Assert.That(cycleTimeViewModel.Tickets.First().FinishedFriendlyText, Is.EqualTo(expectedFinished));
        }

        [Test]
        public void SetsTicketDurationToOneDay()
        {
            const string expectedDuration = "1 Day";

            _ticketReturnedFromDb = new Ticket { CycleTime = new WorkDuration { Days = 1 } };

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory, ticketSummaryFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new CycleTimeQuery());

            Assert.That(cycleTimeViewModel.Tickets.First().Duration, Is.EqualTo(expectedDuration));
        }

        [Test]
        public void SetsTicketDurationToTwoDays()
        {
            const string expectedDuration = "2 Days";

            _ticketReturnedFromDb = new Ticket { CycleTime = new WorkDuration { Days = 2 } };

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory, ticketSummaryFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new CycleTimeQuery());

            Assert.That(cycleTimeViewModel.Tickets.First().Duration, Is.EqualTo(expectedDuration));
        }

        [Test]
        public void SetsTicketSize()
        {
            _ticketReturnedFromDb = new Ticket { CycleTime = new WorkDuration(), Size = 2 };

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory, ticketSummaryFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new CycleTimeQuery());

            Assert.That(cycleTimeViewModel.Tickets.First().Size, Is.EqualTo("2"));
        }

        [Test]
        public void SetsTicketUnknownSizeWhenSizeIsZero()
        {
            _ticketReturnedFromDb = new Ticket { CycleTime = new WorkDuration() };

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory, ticketSummaryFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new CycleTimeQuery());

            Assert.That(cycleTimeViewModel.Tickets.First().Size, Is.EqualTo("?"));
        }

        [Test]
        public void SetsTimePeriods()
        {
            _ticketReturnedFromDb = new Ticket
            {
                CycleTime = new WorkDuration()
            };

            _cycleTimePeriodViewModel = new CycleTimePeriodViewModel(new List<CycleTimePeriod>(0));

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory, ticketSummaryFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new CycleTimeQuery());

            Assert.That(cycleTimeViewModel.CycleTimePeriods, Is.EqualTo(_cycleTimePeriodViewModel));
        }

        [Test]
        public void PassesQueryTimePeriodToTimePeriodFactory()
        {
            _ticketReturnedFromDb = new Ticket
            {
                CycleTime = new WorkDuration()
            };

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory, ticketSummaryFactory);
            cycleTimeViewModelFactory.Build(new CycleTimeQuery { Period = "1234" });

            Assert.That(_queryPassedToTimePeriodFactory, Is.EqualTo("1234"));
        }

        [Test]
        public void SetsReleaseInfo()
        {
            _ticketReturnedFromDb = new Ticket
            {
                CycleTime = new WorkDuration()
            };

            _cycleTimeTicketReleaseInfo = new CycleTimeReleaseViewModel();

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory, ticketSummaryFactory);
            var cycleTimeViewModel = cycleTimeViewModelFactory.Build(new CycleTimeQuery());

            Assert.That(cycleTimeViewModel.Tickets.First().Release, Is.EqualTo(_cycleTimeTicketReleaseInfo));
        }

        [Test]
        public void CycleTimeQueryIsPassedToRepository()
        {
            var query = new CycleTimeQuery();

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;

            new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory, ticketSummaryFactory).Build(query);

            Assert.That(_queryPassedToRepository, Is.EqualTo(query));
        }

        [Test]
        public void SetsSummary()
        {
            var expectedSummaryViewModel = new TicketCycleTimeSummary();

            _cycleTimeSummary = expectedSummaryViewModel;

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeCycleTimeReleaseViewModels cycleReleaseInfoFactory = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;

            var summaryViewModel = new CycleTimeViewModelFactory(ticketRepository,
                cycleReleaseInfoFactory,
                timePeriodViewModelFactory,
                ticketSummaryFactory).Build(new CycleTimeQuery()).Summary;

            Assert.That(summaryViewModel, Is.EqualTo(expectedSummaryViewModel));
        }

        public IEnumerable<Ticket> Get(CycleTimeQuery query)
        {
            _queryPassedToRepository = query;
            return new List<Ticket>
                {
                    _ticketReturnedFromDb
                };
        }

        public CycleTimeReleaseViewModel Build(TicketReleaseInfo ticketReleaseInfo)
        {
            return _cycleTimeTicketReleaseInfo;
        }

        public CycleTimePeriodViewModel Build(string selectedPeriod)
        {
            _queryPassedToTimePeriodFactory = selectedPeriod;
            return _cycleTimePeriodViewModel;
        }

        public TicketCycleTimeSummary Summarise(IEnumerable<Ticket> tickets)
        {
            return _cycleTimeSummary;
        }
    }
}
