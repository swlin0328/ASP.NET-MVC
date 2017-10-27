using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using HtmlAgilityPack;
using System.Text;
using System.Net;
using System.Web.Mvc;
using WebApplication3.Services;
using WebApplication3.ViewModels;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class ExchangeController : Controller
    {
        Finance financeService = new Finance();
        // GET: Exchange
        public ActionResult Index()
        {
            ExchangeView ExchangeList = new ExchangeView();
            ExchangeList.Paging = new ForPaging(1);
            ExchangeList.ExchangeList = financeService.getExchange();

            return View(ExchangeList);
        }

        public ActionResult getExchangeHistory(string Search, int Page = 1)
        {
            ExchangeView ExchangeList = new ExchangeView();
            ExchangeList.Paging = new ForPaging(Page);
            ExchangeList.ExchangeList = financeService.getExchangeList(ExchangeList.Paging, Search);
            ExchangeList.Search = Search;

            return View(ExchangeList);
        }

        public ActionResult UpdateData(string Search, int Page = 1)
        {
            if (String.IsNullOrEmpty(Search))
            {
                TempData["Data"] = "幣別";
                return RedirectToAction("NotFound", "Home");
            }

            financeService.clearData("Exchange", Search, 0);
            financeService.UpdateExchange(Search);

            return RedirectToAction("getExchangeHistory", new { Search = Search, Page = Page });
        }
    }
}

