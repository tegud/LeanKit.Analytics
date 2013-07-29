using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LeanKit.ReleaseManager.Controllers
{
    public class ReleaseDashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
                    var version = response.Headers["X-Version"];

                    return Json(new
                        {
                            Status = "Online",
                            Version = version
                        }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var httpWebException = (HttpWebResponse) ex.Response;
                    if ((int) httpWebException.StatusCode == 418)
                        return Json(new
                        {
                            Status = "Deploying"
                        }, JsonRequestBehavior.AllowGet);

                    return Json(new
                    {
                        Status = "Down"
                    }, JsonRequestBehavior.AllowGet);
                }

                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    return Json(new
                    {
                        Status = "Timeout"
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    Status = "Error"
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
