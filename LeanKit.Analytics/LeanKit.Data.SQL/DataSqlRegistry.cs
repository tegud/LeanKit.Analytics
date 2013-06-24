using LeanKit.IoC;
using Munq;

namespace LeanKit.Data.SQL
{
    public class DataSqlRegistry
    {
        public static void Register(IocContainer ioc)
        {
            ioc.Register<IGetReleasesFromTheDatabase>(i => new ReleaseRepository(i.Resolve<DbConnectionString>()));
            ioc.Register<IGetActivitiesFromTheDatabase>(i => new ActivityRepository(i.Resolve<DbConnectionString>()));
            ioc.Register<ITicketRepository>(i => new TicketsRepository(i.Resolve<DbConnectionString>(), i.Resolve<ICreateTickets>()));
            ioc.Register<IGetReleasedTicketsFromTheDatabase>(
                i => new CompletedTicketsRepository(i.Resolve<DbConnectionString>(), i.Resolve<ICreateTickets>()));
            ioc.Register<ICreateTicketActivities>(i => new TicketActivityFactory(i.Resolve<ICalculateWorkDuration>()));
            ioc.Register<IMakeTicketBlockages>(i => new TicketBlockageFactory(i.Resolve<ICalculateWorkDuration>()));
            ioc.Register<IGetBlockagesFromTheDatabase>(i => new BlockageRepository(i.Resolve<DbConnectionString>()));
            ioc.Register<IGetAllTicketInformation>(i => new FullTicketInformationRepository(i.Resolve<DbConnectionString>(), i.Resolve<ICreateTickets>()));

            ioc.Register<ICreateTickets>(i =>
                {
                    var ticketStartDateFactory = i.Resolve<ICalculateTicketMilestone>(Module.TicketStartDateFactory);
                    var ticketFinishDateFactory = i.Resolve<ICalculateTicketMilestone>(Module.TicketFinishDateFactory);
                    var ticketCycleTimeDurationFactory = i.Resolve<ICalculateWorkDuration>(Module.TicketCycleTimeDuration);
                    var ticketActivityFactory = i.Resolve<ICreateTicketActivities>();
                    var ticketCurrentActivityFactory = new CurrentActivityFactory();
                    var ticketBlockagesFactory = i.Resolve<IMakeTicketBlockages>();

                    return new TicketFactory(ticketStartDateFactory,
                                             ticketFinishDateFactory,
                                             ticketActivityFactory,
                                             ticketCycleTimeDurationFactory,
                                             ticketCurrentActivityFactory,
                                             ticketBlockagesFactory);
                });
        }
    }
}