namespace LeanKit.APIClient.API
{
    public class LeankitBoardCard
    {
        public int Id { get; set; }

        public string ExternalCardID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Size { get; set; }

        public bool Active { get; set; }

        public bool IsBlocked { get; set; }

        public string BlockReason { get; set; }

        public string BlockStateChangeDate { get; set; }

        public string LastMove { get; set; }

        public string Tags { get; set; }
    }
}