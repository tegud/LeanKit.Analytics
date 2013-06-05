using System;
using LeanKit.Utilities.DateAndTime;
using Munq;
using LeanKit.IoC;

namespace LeanKit.Data
{
    public static class DataRegistry
    {
        public static void Register(IocContainer ioc)
        {
            ioc.Register<ICalculateWorkDuration>(
                i => new WorkDurationFactory(new DateTime[0], new WorkDayDefinition { Start = 9, End = 7 }));
            ioc.Register<ICalculateWorkDuration>(Module.TicketCycleTimeDuration,
                                                 i =>
                                                 new TicketCycleTimeDurationFactory(i.Resolve<ICalculateWorkDuration>(),
                                                                                    i.Resolve<IKnowTheCurrentDateAndTime>()));
            ioc.Register<IActivitySpecification, ActivityIsInProgressSpecification>(Module.ActivityInProgressSpecification);
            ioc.Register<IActivitySpecification, ActivityIsLiveSpecification>(Module.ActivityIsLiveSpecification);
            ioc.Register<ICalculateTicketMilestone>(Module.TicketStartDateFactory,
                                                    i =>
                                                    new TicketStartDateFactory(
                                                        i.Resolve<IActivitySpecification>(Module.ActivityInProgressSpecification)));
            ioc.Register<ICalculateTicketMilestone>(Module.TicketFinishDateFactory,
                                                    i =>
                                                    new TicketFinishDateFactory(
                                                        i.Resolve<IActivitySpecification>(Module.ActivityIsLiveSpecification)));
            ioc.Register<IFindTheCurrentActivity, CurrentActivityFactory>();
        }
    }
}