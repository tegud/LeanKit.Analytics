using System.Web.Mvc;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.IoC;
using LeanKit.Utilities;
using Munq.MVC3;

[assembly: WebActivator.PreApplicationStartMethod(
	typeof(LeanKit.ReleaseManager.App_Start.MunqMvc3Startup), "PreStart")]

namespace LeanKit.ReleaseManager.App_Start 
{
    public static class MunqMvc3Startup
	{
	    public static void PreStart() 
        {
			DependencyResolver.SetResolver(new MunqDependencyResolver());

		    var ioc = MunqDependencyResolver.Container;

	        ioc.Register(Module.ConnectionString, i => MvcApplication.ConnectionString);

		    UtilitiesRegistry.Register(ioc);
            DataRegistry.Register(ioc);
	        DataSqlRegistry.Register(ioc);
        }
	}
}
 

