using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    public class RSSController : Controller
    {
        // GET: RSS
        public ActionResult RSSContent()
        {
            RSS RSSService = new RSS();
            NewsView StockNews = new NewsView();

            StockNews = RSSService.RSSNews();
            
            return PartialView(StockNews);
        }
    }
}