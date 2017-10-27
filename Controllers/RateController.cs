using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    public class RateController : Controller
    {
        Finance RateService = new Finance();
        // GET: Rate
        public ActionResult MainCountry()
        {
            RateListView Rate = new RateListView();
            Rate.RateList = RateService.getRate();

            return PartialView(Rate);
        }
    }
}