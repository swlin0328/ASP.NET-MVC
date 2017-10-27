using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            /* try
             {
                 int a = 1;
                 int b = 0;
                 int c = a / b;
             }
             catch (Exception ex)
             {
                 ViewBag.result = ex.Message;
                 logger.Error(LogUtility.GetExceptionDetails(ex));
             }*/

            //  throw new NotImplementedException();
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            ViewBag.Data = TempData["Data"];
            return View();
        }
    }
}