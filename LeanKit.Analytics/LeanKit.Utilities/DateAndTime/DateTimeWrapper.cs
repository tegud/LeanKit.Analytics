namespace LeanKit.Utilities.DateAndTime
{
    public class DateTimeWrapper : IKnowTheCurrentDateAndTime
    {
        public System.DateTime Now()
        {
            return System.DateTime.Now;
        }
    }
}