using System.Linq;
using LeanKit.Data;
using LeanKit.Data.SQL;

namespace LeanKit.ReleaseManager.Models.CycleTime
{
    public class CycleTimeViewModelFactory : IBuildCycleTimeViewModels
    {
        private readonly IGetReleasedTicketsFromTheDatabase _ticketRepository;
        private readonly IMakeTimePeriodViewModels _timePeriodViewModelFactory;
        private readonly ISummariseTicketCycleTimeInformation _ticketSummaryFactory;
        private readonly IBuildListOfCycleTimeItems _ticketCycleTimeItemsFactory;

        public CycleTimeViewModelFactory(IGetReleasedTicketsFromTheDatabase ticketRepository, 
            IMakeTimePeriodViewModels timePeriodViewModelFactory, 
            ISummariseTicketCycleTimeInformation ticketSummaryFactory, 
            IBuildListOfCycleTimeItems ticketCycleTimeItemsFactory)
        {
            _timePeriodViewModelFactory = timePeriodViewModelFactory;
            _ticketSummaryFactory = ticketSummaryFactory;
            _ticketCycleTimeItemsFactory = ticketCycleTimeItemsFactory;
            _ticketRepository = ticketRepository;
        }

        public CycleTimeViewModel Build(CycleTimeQuery query)
        {
            var tickets = _ticketRepository.Get(query).ToArray();
            var cycleTimeTicketItems = _ticketCycleTimeItemsFactory.Build(tickets);

            return new CycleTimeViewModel
            {
                Tickets = cycleTimeTicketItems,
                CycleTimePeriods = _timePeriodViewModelFactory.Build(query.Period),
                Summary = _ticketSummaryFactory.Summarise(tickets)
            };
        }
    }
}