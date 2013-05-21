using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LeanKit.APIClient.API
{
    public class ApiCaller
    {
        public string Account { get; set; }

        public string BoardId { get; set; }

        public ApiCredentials Credentials { get; set; }

        public T GetBoardResponse<T>(string command)
        {
            var address = string.Format("http://{0}.leankitkanban.com/Kanban/Api/{1}/{2}", Account, command, BoardId);
            var output = new ApiRequest(Credentials, address).GetResponse();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(output);

            return apiResponse.ReplyData.First();
        }

        public LeanKitCardHistory GetHistoryResponse<T>(int cardId)
        {
            var address = string.Format("http://{0}.leankitkanban.com/Kanban/API/Card/History/{1}/{2}", Account, BoardId, cardId);
            var output = new ApiRequest(Credentials, address).GetResponse();

            return new LeanKitCardHistory();
        }
    }

    public class HistoryReplyDataWrapper : IEnumerable<LeanKitCardHistory>
    {
        private readonly LeanKitCardHistory[] _items;

        public HistoryReplyDataWrapper(LeanKitCardHistory[] items)
        {
            _items = items;
        }

        public IEnumerator<LeanKitCardHistory> GetEnumerator()
        {
            return (IEnumerator<LeanKitCardHistory>) _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class LeanKitCardHistory
    {
    }
}