using LeanKit.IoC;
using Munq;

namespace LeanKit.Data.SQL
{
    public class DataSqlRegistry
    {
        public static void Register(IocContainer ioc)
        {
            ioc.Register<IGetReleasesFromTheDatabase>(i => new ReleaseRepository(i.Resolve<string>(Module.ConnectionString)));
            ioc.Register<IGetActivitiesFromTheDatabase>(i => new ActivityRepository(i.Resolve<string>(Module.ConnectionString)));
            ioc.Register<ITicketRepository>(i => new TicketsRepository(i.Resolve<string>(Module.ConnectionString), i.Resolve<ICreateTickets>()));
            ioc.Register<IGetReleasedTicketsFromTheDatabase>(
                i => new CompletedTicketsRepository(i.Resolve<string>(Module.ConnectionString), i.Resolve<ICreateTickets>()));

            ioc.Register<ICreateTicketActivities>(i => new TicketActivityFactory(i.Resolve<ICalculateWorkDuration>()));

            ioc.Register<ICreateTickets>(i =>
                {
                    var ticketStartDateFactory = i.Resolve<ICalculateTicketMilestone>(Module.TicketStartDateFactory);
                    var ticketFinishDateFactory = i.Resolve<ICalculateTicketMilestone>(Module.TicketFinishDateFactory);
                    var ticketCycleTimeDurationFactory = i.Resolve<ICalculateWorkDuration>(Module.TicketCycleTimeDuration);
                    var ticketActivityFactory = i.Resolve<ICreateTicketActivities>();
                    var ticketCurrentActivityFactory = new CurrentActivityFactory();

                    return new TicketFactory(ticketStartDateFactory,
                                             ticketFinishDateFactory,
                                             ticketActivityFactory,
                                             ticketCycleTimeDurationFactory,
                                             ticketCurrentActivityFactory);
                });
        }
    }
}