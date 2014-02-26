using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LeanKit.ReleaseManager.Models.Forecasting
{
    public class ListOfWeeks : IEnumerable<Week>
    {
        private readonly List<Week> _weeks = new List<Week>();

        public ListOfWeeks(DateTime startOfWeekOne, int numberOfWeeks)
        {
            for (var x = 0; x < numberOfWeeks; x++)
            {
                _weeks.Add(new Week(startOfWeekOne.AddDays(x * 7), startOfWeekOne.AddDays((x * 7) + 6)));
            }
        }

        public DateTime StartOfPeriod
        {
            get { return _weeks.Min(w => w.Start); }
        }

        public DateTime EndOfPeriod
        {
            get { return _weeks.Max(w => w.End); }
        }

        public IEnumerator<Week> GetEnumerator()
        {
            return _weeks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}