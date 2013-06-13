namespace LeanKit.ReleaseManager.Models.CycleTime
{
    public class CycleTimeReleaseViewModel
    {
        public static CycleTimeReleaseViewModel NotReleased = new CycleTimeReleaseViewModel();

        public string Name { get; set; }

        public int Id { get; set; }
    }
}