using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class CycleTimeViewModelFactoryTests : IGetReleasedTicketsFromTheDatabase, IMakeCycleTimeReleaseViewModels, IMakeTimePeriodViewModels
    {
        private Ticket _ticketReturnedFromDb;
        private CycleTimeQuery _queryPassedToRepository;
        private CycleTimeReleaseViewModel _cycleTimeTicketReleaseInfo;
        private CycleTimePeriodViewModel _cycleTimePeriodViewModel;
        private string _queryPassedToTimePeriodFactory;

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

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory);
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

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory);
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

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory);
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

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory);
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

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory);
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

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory);
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

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory);
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

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory);
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

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory);
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

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory);
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

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory);
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

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory);
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

            new CycleTimeViewModelFactory(ticketRepository, cycleReleaseInfoFactory, timePeriodViewModelFactory).Build(query);

            Assert.That(_queryPassedToRepository, Is.EqualTo(query));
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
    }
}
