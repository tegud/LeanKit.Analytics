using System.Collections.Generic;

namespace LeanKit.ReleaseManager.Models
{
    public class CreateReleaseModel
    {
        public IEnumerable<DateOption> DateOptions { get; set; }
    }
}