using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LeanKit.Analytics.Models.ViewModels
{
    public class HomeViewModel
    {
        public WasteGraph MainWasteGraph { get; set; }
    }

    public class WasteGraph
    {
        public IEnumerable<WasteGraphActivity> Activities { get; private set; }

        public WasteGraph(IEnumerable<WasteGraphActivity> activities)
        {
            Activities = activities;
        }
    }

    public class WasteGraphActivity
    {
        public string Activity { get; set; }

        public double Percent { get; set; }

        public string FormattedPercent { get { return Percent.ToString("0.00"); } }

        public bool IsWaste { get; set; }

        public string ClassName
        {
            get { return Activity.Replace(" ", string.Empty); }
        }
    }
}