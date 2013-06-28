using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace LeanKit.APIClient.API
{
    public class ApiRequest
    {
        private readonly ApiCredentials _credentials;
        private readonly string _address;

        public ApiRequest(ApiCredentials credentials, string address)
        {
            _credentials = credentials;
            _address = address;
        }

        public string GetResponse()
        {
            var request = (HttpWebRequest)WebRequest.Create(_address);

            request.Method = "GET";
            request.Credentials = new NetworkCredential(_credentials.Username, _credentials.Password);
            request.PreAuthenticate = true;
            request.Timeout = 15000;
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);

            var output = string.Empty;
            try
            {
                using (var response = request.GetResponse())
                {
                    using (var stream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1252)))
                    {
                        output = stream.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = (HttpWebResponse) ex.Response;

                    if(response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new ApiCredentialsIncorrectException();
                    }

                    using (var stream = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        output = stream.ReadToEnd();
                    }
                }
                else if (ex.Status == WebExceptionStatus.Timeout)
                {
                    output = "Request timeout is expired.";
                }
            }

            return output;
        }
    }

    public class ApiCredentialsIncorrectException : Exception
    {
    }
}