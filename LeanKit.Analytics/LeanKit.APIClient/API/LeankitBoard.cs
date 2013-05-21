namespace LeanKit.APIClient.API
{
    public class LeankitBoard
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public LeankitBoardLane[] Lanes { get; set; }
    }
}