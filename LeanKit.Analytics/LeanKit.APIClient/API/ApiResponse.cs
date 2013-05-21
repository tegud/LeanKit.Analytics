namespace LeanKit.APIClient.API
{
    public class ApiResponse<T>
    {
        public int ReplyCode { get; set; }

        public string ReplyText { get; set; }

        public T[] ReplyData { get; set; }
    }
}