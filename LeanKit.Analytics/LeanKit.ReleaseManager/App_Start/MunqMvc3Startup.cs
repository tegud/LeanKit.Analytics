using System;
using System.Collections;
using System.Configuration;
using System.Web.Mvc;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.IoC;
using LeanKit.ReleaseManager.Models;
using LeanKit.Utilities;
using LeanKit.Utilities.Collections;
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

            ioc.Register(Module.ConnectionString, i => MvcApplication.ConnectionString);

            ioc.Register<IIdentifyWorkDays, DateIsWorkDaySpecification>();
            ioc.Register<IMakeListsOfDateOptions, DateOptionsFactory>();
            ioc.Register<IRotateThroughASetOfColours>(i => new ColourPalette(Configuration.GetReleaseColours()));

            UtilitiesRegistry.Register(ioc);
            DataRegistry.Register(ioc);
            DataSqlRegistry.Register(ioc);
        }
    }
}

