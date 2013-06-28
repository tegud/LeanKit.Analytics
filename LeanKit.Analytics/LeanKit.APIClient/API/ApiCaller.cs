using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LeanKit.APIClient.API
{
    public interface IApiCaller
    {
        LeankitBoard GetBoard();
        IEnumerable<LeanKitCardHistory> GetCardHistory(int cardId);
        LeankitBoardLaneWrapper GetBoardArchive();
    }

    public class ApiCaller : IApiCaller
    {
        public string Account { get; set; }

        public string BoardId { get; set; }

        public ApiCredentials Credentials { get; set; }

        public LeankitBoard GetBoard()
        {
            var address = string.Format("{0}Boards/{1}", BaseApiUrl(), BoardId);

            return GetResponse<LeankitBoard>(address);
        }

        public IEnumerable<LeanKitCardHistory> GetCardHistory(int cardId)
        {
            var address = string.Format("{0}Card/History/{1}/{2}", BaseApiUrl(), BoardId, cardId);

            return GetResponse<LeanKitCardHistory[]>(address);
        }

        private string BaseApiUrl()
        {
            return string.Format("http://{0}.leankitkanban.com/Kanban/Api/", Account);
        }

        private T GetResponse<T>(string address)
        {
            var output = new ApiRequest(Credentials, address).GetResponse();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(output);

            if(apiResponse.ReplyCode == 1000)
            {
                throw new NoAccessToSpecifiedBoardException();
            }

            return apiResponse.ReplyData.First();
        }

        public LeankitBoardLaneWrapper GetBoardArchive()
        {
            var address = string.Format("{0}Board/{1}/Archive", BaseApiUrl(), BoardId);

            var output = new ApiRequest(Credentials, address).GetResponse();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<IEnumerable<LeankitBoardLaneWrapper>>>(output);

            return apiResponse.ReplyData.First().First();
        }

        
    }

    internal class NoAccessToSpecifiedBoardException : Exception
    {
    }

    public class LeankitBoardLaneWrapper
    {
        public LeankitBoardLane Lane { get; set; }

        public LeankitBoardLaneWrapper[] ChildLanes { get; set; }
    }
}