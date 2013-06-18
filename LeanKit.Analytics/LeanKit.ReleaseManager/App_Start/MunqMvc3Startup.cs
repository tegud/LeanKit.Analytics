using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.IoC;
using LeanKit.ReleaseManager.Controllers;
using LeanKit.ReleaseManager.Models;
using LeanKit.ReleaseManager.Models.CycleTime;
using LeanKit.ReleaseManager.Models.Releases;
using LeanKit.ReleaseManager.Models.TimePeriods;
using LeanKit.Utilities;
using LeanKit.Utilities.Collections;
using LeanKit.Utilities.DateAndTime;
using Munq.MVC3;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof(LeanKit.ReleaseManager.App_Start.MunqMvc3Startup), "PreStart")]

namespace LeanKit.ReleaseManager.App_Start
{
    public static class Configuration
    {
        public static string[] GetReleaseColours()
        {
            var configurationSection = (Hashtable)ConfigurationManager.GetSection("releaseColours");
            
            if(!configurationSection.ContainsKey("colours"))
            {
                return new [] { "#CCCCCC" };
            }

            return configurationSection.GetValue("colours").Split(',');
        }
    }

    public static class MunqMvc3Startup
    {
        public static void PreStart()
        {
            DependencyResolver.SetResolver(new MunqDependencyResolver());

            var ioc = MunqDependencyResolver.Container;

            ioc.Register(i => MvcApplication.ConnectionString);

            UtilitiesRegistry.Register(ioc);
            ioc.Register(i => new IDefineATimePeriodItem[]
                {
                    new LastXDaysPeriodItem(0)
                });

            ioc.Register<IIdentifyWorkDays, DateIsWorkDaySpecification>();
            ioc.Register<IMakeListsOfDateOptions, DateOptionsFactory>();
            ioc.Register<IRotateThroughASetOfColours>(i => new ColourPalette(Configuration.GetReleaseColours()));
            ioc.Register<IBuildIncludedTicketsForNewReleases, NewReleaseIncludedTicketsBuilders>();
            ioc.Register<IParsePlannedReleaseDate, PlannedDateParser>();
            ioc.Register<IBuildNewReleaseRecords, CreateReleaseReleaseRecordFactory>();
            ioc.Register<IBuildReleaseViewModels, ReleaseViewModelFactory>();
            ioc.Register<IBuildCycleTimeViewModels, CycleTimeViewModelFactory>();
            ioc.Register<IMakeCycleTimeReleaseViewModels, CycleTimeReleaseViewModelFactory>();

            ioc.Register<IMatchATimePeriod>("MatchWeekCommencing", i => new MatchWeekCommencingTimePeriod(i.Resolve<IKnowTheCurrentDateAndTime>()));
            ioc.Register<IMatchATimePeriod>("MatchDaysBefore", i => new MatchDaysBeforeTimePeriod(i.Resolve<IKnowTheCurrentDateAndTime>()));
            ioc.Register<IMatchATimePeriod>("MatchKeyword", i => new MatchKeywordTimePeriod(i.Resolve<IKnowTheCurrentDateAndTime>()));

            ioc.Register<IMakeCycleTimeQueries>(i => new CycleTimeQueryFactory(new[]
                {
                    i.Resolve<IMatchATimePeriod>("MatchWeekCommencing"),
                    i.Resolve<IMatchATimePeriod>("MatchDaysBefore"),
                    i.Resolve<IMatchATimePeriod>("MatchKeyword")
                }, "30"));

            ioc.Register<IMakeTimePeriodViewModels, CycleTimePeriodViewModelFactory>();
            ioc.Register<ISummariseTicketCycleTimeInformation, SummariseTicketCycleTimeInformation>();
            ioc.Register<IBuildListOfCycleTimeItems, CycleTimeListOfTicketsViewModelFactory>();

            DataRegistry.Register(ioc);
            DataSqlRegistry.Register(ioc);
        }
    }
}


