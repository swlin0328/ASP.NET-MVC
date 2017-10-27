using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Web.Mvc;
using WebApplication3.Services;
using WebApplication3.ViewModels;
using WebApplication3.Models;
using ScrapySharp.Extensions;

namespace WebApplication3.Controllers
{
    public class StockController : Controller
    {
        Finance financeService = new Finance();
        Currency CurrencyService = new Currency();
        // GET: Stock
        public ActionResult Index()
        {
            MarketView Market = new MarketView();
            Market.MarketList = financeService.getMarketList();
            Market.VIXList = new string[10];
            Market.VIXList = financeService.getVIXList();
            Market.LegalPerson = new string[10];
            Market.LegalPerson = financeService.getLegalPerson();
            Market.ForceRatio = new decimal[3];
            Market.ForceRatio = financeService.getForceRatio();

            return View(Market);
        }

        public ActionResult getStockItem(string Search)
        {
            StockItem stockItem = new StockItem();
            stockItem = financeService.getStock(Search);
            stockItem.Search = Search;

            return View(stockItem);
        }

        public ActionResult getStockHistory(string Search, int Page = 1)
        {
            StockView StockList = new StockView();
            StockList.Paging = new ForPaging(Page);
            StockList.StockList = financeService.getStockList(StockList.Paging, Search);
            StockList.Search = Search;

            return View(StockList);
        }

        public ActionResult UpdateData(string Search, int Page = 1)
        {
            if (String.IsNullOrEmpty(Search))
            {
                TempData["Data"] = "股票";
                return RedirectToAction("NotFound", "Home");
            }

            int Num = int.Parse(Search);
            financeService.clearData("Stock", "", Num);
            financeService.UpdateStock(int.Parse(Search));

            return RedirectToAction("getStockHistory", new { Search = Search, Page = Page });
        }

        public ActionResult autoRefresh(string Search, int Page = 1)
        {
            financeService.autoStockRefresh();
            return RedirectToAction("autoRefreshStock");
        }

        public ActionResult autoRefreshStock(string Search, int Page = 1)
        {
            financeService.autoExchangeRefresh();
            financeService.autoCYQRefresh();
            financeService.UpdateMacroEconomic();
            financeService.UpdateBDI_CRB();

            return RedirectToAction("autoRefreshCurrency");
        }

        public ActionResult autoRefreshCurrency(string Search, int Page = 1)
        {
            financeService.autoVoucherRefresh();
            financeService.UpdateBullAndBear();
            CurrencyService.UpdateLibor();
            CurrencyService.UpdateTaifex();

            return RedirectToAction("Monetary", "Currency");
        }

        public ActionResult StockItemTech(int Num)
        {
            TechData TechList = new TechData();
            ForPaging page = new ForPaging(1);
            TechList.Search = Num;
            TechList.StockList = financeService.getStockList(page, Num.ToString());
            TechList.VoucherList = financeService.getVoucher(Num.ToString());
            TechList.CYQList = financeService.getCYQ(Num.ToString());
            TechList.MonetaryList = CurrencyService.getMonetaryList();
            TechList.LiborList = CurrencyService.getLiborList();
            TechList.CRB_BDIList = financeService.getCRB_BDI();

            return View(TechList);
        }
    }
}