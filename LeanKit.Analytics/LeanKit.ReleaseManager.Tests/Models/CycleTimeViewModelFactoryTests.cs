using System.Collections.Generic;
using System.Linq;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;
using LeanKit.ReleaseManager.Models.CycleTime;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class CycleTimeViewModelFactoryTests : IGetReleasedTicketsFromTheDatabase, IMakeCycleTimeReleaseViewModels, IMakeTimePeriodViewModels, ISummariseTicketCycleTimeInformation, IBuildListOfCycleTimeItems
    {
        private Ticket _ticketReturnedFromDb;
        private CycleTimeQuery _queryPassedToRepository;
        private CycleTimeReleaseViewModel _cycleTimeTicketReleaseInfo;
        private CycleTimePeriodViewModel _cycleTimePeriodViewModel;
        private string _queryPassedToTimePeriodFactory;
        private TicketCycleTimeSummary _cycleTimeSummary;
        private IEnumerable<CycleTimeTicketItem> _listOfCycleTimeTicketItemsReturnedFromFactory;

        [Test]
        public void SetsTimePeriods()
        {
            _ticketReturnedFromDb = new Ticket
            {
                CycleTime = new WorkDuration()
            };

            _cycleTimePeriodViewModel = new CycleTimePeriodViewModel(new List<CycleTimePeriod>(0));

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;
            IBuildListOfCycleTimeItems listOfCycleTimeItemsFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, timePeriodViewModelFactory, ticketSummaryFactory, listOfCycleTimeItemsFactory);
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
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;
            IBuildListOfCycleTimeItems listOfCycleTimeItemsFactory = this;

            var cycleTimeViewModelFactory = new CycleTimeViewModelFactory(ticketRepository, timePeriodViewModelFactory, ticketSummaryFactory, listOfCycleTimeItemsFactory);
            cycleTimeViewModelFactory.Build(new CycleTimeQuery { Period = "1234" });

            Assert.That(_queryPassedToTimePeriodFactory, Is.EqualTo("1234"));
        }

        [Test]
        public void CycleTimeQueryIsPassedToRepository()
        {
            var query = new CycleTimeQuery();

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;
            IBuildListOfCycleTimeItems listOfCycleTimeItemsFactory = this;

            new CycleTimeViewModelFactory(ticketRepository, timePeriodViewModelFactory, ticketSummaryFactory, listOfCycleTimeItemsFactory).Build(query);

            Assert.That(_queryPassedToRepository, Is.EqualTo(query));
        }

        [Test]
        public void SetsSummary()
        {
            var expectedSummaryViewModel = new TicketCycleTimeSummary();

            _cycleTimeSummary = expectedSummaryViewModel;

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;
            IBuildListOfCycleTimeItems listOfCycleTimeItemsFactory = this;

            var summaryViewModel = new CycleTimeViewModelFactory(ticketRepository,
                timePeriodViewModelFactory,
                ticketSummaryFactory, listOfCycleTimeItemsFactory).Build(new CycleTimeQuery()).Summary;

            Assert.That(summaryViewModel, Is.EqualTo(expectedSummaryViewModel));
        }

        [Test]
        public void SetsListTickets()
        {
            IEnumerable<CycleTimeTicketItem> expectedListOfCycleTimeTicketItems = new List<CycleTimeTicketItem>();

            _listOfCycleTimeTicketItemsReturnedFromFactory = expectedListOfCycleTimeTicketItems;

            IGetReleasedTicketsFromTheDatabase ticketRepository = this;
            IMakeTimePeriodViewModels timePeriodViewModelFactory = this;
            ISummariseTicketCycleTimeInformation ticketSummaryFactory = this;
            IBuildListOfCycleTimeItems listOfCycleTimeItemsFactory = this;

            var listOfCycleTimeTicketItems = new CycleTimeViewModelFactory(ticketRepository,
                timePeriodViewModelFactory,
                ticketSummaryFactory, listOfCycleTimeItemsFactory).Build(new CycleTimeQuery()).Tickets;

            Assert.That(listOfCycleTimeTicketItems, Is.EqualTo(expectedListOfCycleTimeTicketItems));
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

        public IEnumerable<CycleTimeTicketItem> Build(IEnumerable<Ticket> tickets)
        {
            return _listOfCycleTimeTicketItemsReturnedFromFactory;
        }
    }
}
