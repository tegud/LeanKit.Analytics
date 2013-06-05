using System;
using System.Web.Mvc;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.Utilities.DateAndTime;
using Munq.MVC3;

[assembly: WebActivator.PreApplicationStartMethod(
	typeof(LeanKit.ReleaseManager.App_Start.MunqMvc3Startup), "PreStart")]

namespace LeanKit.ReleaseManager.App_Start {

    public static class Module
    {
        public static readonly string WorkDurationFactory = "WorkDurationFactory";
        public static readonly string ActivityInProgressSpecification = "ActivityInProgressSpecification";
        public static readonly string ActivityIsLiveSpecification = "ActivityIsLiveSpecification";
        public static readonly string TicketCycleTimeDuration = "TicketCycleTimeDuration";
        public static readonly string TicketStartDateFactory = "TicketStartDateFactory";
        public static readonly string TicketFinishDateFactory = "TicketFinishDateFactory";
    }

	public static class MunqMvc3Startup
	{
	    public static void PreStart() 
        {
			DependencyResolver.SetResolver(new MunqDependencyResolver());

		    var ioc = MunqDependencyResolver.Container;

		    ioc.Register<IKnowTheCurrentDateAndTime, DateTimeWrapper>();

            ioc.Register<ICalculateWorkDuration>(i => new WorkDurationFactory(new DateTime[0], new WorkDayDefinition { Start = 9, End = 7 }));
            ioc.Register<ICalculateWorkDuration>(Module.TicketCycleTimeDuration,
                i => new TicketCycleTimeDurationFactory(i.Resolve<ICalculateWorkDuration>(), i.Resolve<IKnowTheCurrentDateAndTime>()));
	        ioc.Register<IActivitySpecification, ActivityIsInProgressSpecification>(Module.ActivityInProgressSpecification);
            ioc.Register<IActivitySpecification, ActivityIsLiveSpecification>(Module.ActivityIsLiveSpecification);
            ioc.Register<ICalculateTicketMilestone>(Module.TicketStartDateFactory, 
                i => new TicketStartDateFactory(i.Resolve<IActivitySpecification>(Module.ActivityInProgressSpecification)));
            ioc.Register<ICalculateTicketMilestone>(Module.TicketFinishDateFactory,
                i => new TicketFinishDateFactory(i.Resolve<IActivitySpecification>(Module.ActivityIsLiveSpecification)));
            ioc.Register<IFindTheCurrentActivity, CurrentActivityFactory>();

            ioc.Register<ICreateTicketActivities>(i => new TicketActivityFactory(i.Resolve<ICalculateWorkDuration>()));


            ioc.Register<ICreateTickets>(i =>
		        {
                    var ticketStartDateFactory = i.Resolve<ICalculateTicketMilestone>(Module.TicketStartDateFactory);
                    var ticketFinishDateFactory = i.Resolve<ICalculateTicketMilestone>(Module.TicketFinishDateFactory);
		            var ticketCycleTimeDurationFactory = i.Resolve<ICalculateWorkDuration>(Module.TicketCycleTimeDuration);
		            var ticketActivityFactory = i.Resolve<ICreateTicketActivities>();
		            var ticketCurrentActivityFactory = new CurrentActivityFactory();

		            return new Data.SQL.TicketFactory(ticketStartDateFactory, 
                        ticketFinishDateFactory, 
                        ticketActivityFactory,
                        ticketCycleTimeDurationFactory, 
                        ticketCurrentActivityFactory);
		        });
		}
	}
}
 

