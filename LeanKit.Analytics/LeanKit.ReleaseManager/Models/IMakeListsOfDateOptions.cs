using System.Collections.Generic;
using LeanKit.ReleaseManager.Controllers;

namespace LeanKit.ReleaseManager.Models
{
    public interface IMakeListsOfDateOptions
    {
        IEnumerable<DateOption> BuildDateOptions(int numberOfDaysToDisplay);
    }
}