using System.Net;
using System.Web.Mvc;
using LeanKit.ReleaseManager.Models.ReleaseDashboard;

namespace LeanKit.ReleaseManager.Controllers
{
    public class ReleaseDashboardController : Controller
    {
        public ActionResult Index()
        {
            var viewModel = new ReleaseDashboardViewModelFactory().Build();
            return View("Index", viewModel);
        }

        public ActionResult Status(string server)
        {
            var url = string.Format("http://{0}.tlrg.org/en/loadbalancercheck.mvc", server);
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";
            request.Timeout = 2500;

            try
            {
                using (var response = request.GetResponse())
                {
                    var version = GetCodeRevision(response.Headers["X-Version"]);

                    return JsonResponseWithVersion(version, "Online");
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var httpWebException = (HttpWebResponse)ex.Response;

                    var version = GetCodeRevision(ex.Response.Headers["X-Version"]);

                    if ((int)httpWebException.StatusCode == 418)
                        return JsonResponseWithVersion(version, "Deploying");

                    return JsonResponseWithoutVersion("Down");
                }

                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    return JsonResponseWithoutVersion("Timeout");
                }

                return JsonResponseWithoutVersion("Error");
            }
        }

        private JsonResult JsonResponseWithoutVersion(string status)
        {
            return Json(new
                {
                    Status = status
                }, JsonRequestBehavior.AllowGet);
        }

        private JsonResult JsonResponseWithVersion(string version, string status)
        {
            return Json(new
                {
                    Status = status,
                    Version = version
                }, JsonRequestBehavior.AllowGet);
        }

        public string GetCodeRevision(string fullVersionString)
        {
            if (!fullVersionString.Contains("_"))
            {
                return fullVersionString;
            }

            return fullVersionString.Split('_')[0];
        }
    }
}
