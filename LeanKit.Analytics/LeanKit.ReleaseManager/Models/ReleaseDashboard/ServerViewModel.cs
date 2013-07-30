namespace LeanKit.ReleaseManager.Models.ReleaseDashboard
{
    public class ServerViewModel
    {
        public string Host { get { return string.Format("telweb{0:000}P", Id); } }

        public int Id { get; set; }
    }
}