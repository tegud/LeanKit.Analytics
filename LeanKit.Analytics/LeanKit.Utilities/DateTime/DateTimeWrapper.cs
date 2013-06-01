namespace LeanKit.Utilities.DateTime
{
    public class DateTimeWrapper : IKnowTheCurrentDateAndTime
    {
        public System.DateTime Now()
        {
            return System.DateTime.Now;
        }
    }
}