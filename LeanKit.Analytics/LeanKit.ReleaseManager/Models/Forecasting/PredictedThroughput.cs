namespace LeanKit.ReleaseManager.Models.Forecasting
{
    public class PredictedThroughput
    {
        public int Releases { get; private set; }

        public int Tickets { get; private set; }

        public int Complexity { get; private set; }

        public PredictedThroughput(int releases, int tickets, int forecastComplexity)
        {
            Releases = releases;
            Tickets = tickets;
            Complexity = forecastComplexity;
        }
    }
}