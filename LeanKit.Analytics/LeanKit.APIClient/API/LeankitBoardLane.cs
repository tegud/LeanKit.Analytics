namespace LeanKit.APIClient.API
{
    public class LeankitBoardLane
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int Index { get; set; }

        public bool Active { get; set; }

        public string Title { get; set; }

        public int CardLimit { get; set; }

        public LeankitBoardCard[] Cards { get; set; }
    }
}