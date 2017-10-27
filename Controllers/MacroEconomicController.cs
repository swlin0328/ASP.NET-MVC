using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Services;
using WebApplication3.ViewModels;
using WebApplication3.Models;
using System.IO;
using HtmlAgilityPack;
using System.Text;
using System.Net;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace WebApplication3.Controllers
{
    public class MacroEconomicController : Controller
    {
        Finance EconomicService = new Finance();
        // GET: MacroEconomic
        public ActionResult Index()
        {
            MacroEconomicView EconomicList = new MacroEconomicView();
            EconomicList.MacroEconomicList = EconomicService.getEconomic();
            EconomicService.UpdateBDI_CRB();

            return View(EconomicList);
        }
    }
}