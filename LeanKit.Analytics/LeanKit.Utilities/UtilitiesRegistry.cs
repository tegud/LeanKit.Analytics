using LeanKit.Utilities.DateAndTime;
using Munq;

namespace LeanKit.Utilities
{
    public static class UtilitiesRegistry
    {
        public static void Register(IocContainer ioc)
        {
            ioc.Register<IKnowTheCurrentDateAndTime, DateTimeWrapper>();
        }
    }
}