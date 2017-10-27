using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Services;
using Spire.Xls;
using System.IO;
using System.Net;
using WebApplication3.Models;
using HtmlAgilityPack;
using System.Text;
using ScrapySharp.Extensions;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types

namespace WebApplication3.Controllers
{
    public class CurrencyController : Controller
    {
        Currency CurrencyService = new Currency();
        FileService xlsService = new FileService();
        FinanceEntities db = new FinanceEntities();

        // GET: Currency
        public ActionResult Index()
        {
            /*CurrencyService.ClearData("Libor");
            CurrencyService.UpdateLibor();*/
            CurrencyService.UpdateTaifex();
            return View();
        }

        public ActionResult Monetary()
        {
            Workbook doc = new Workbook();
            WebClient webClient = new WebClient();
            using (MemoryStream ms = new MemoryStream(webClient.DownloadData("http://www.cbc.gov.tw/public/Data/782416272371.xls")))
            {
                doc.LoadFromStream(ms);
            }

            MemoryStream stram = new MemoryStream();
            doc.SaveToStream(stram, FileFormat.Version97to2003);
            xlsService.UploadXLSAzure("mvc","Upload", "Monetary.xls", stram);

            //doc.SaveToFile("https://uploadmvc.blob.core.windows.net/mvc/Upload/Monetary.xls", FileFormat.Version97to2003);
            //doc.SaveToFile(Server.MapPath("~/Upload/Monetary.xls"), FileFormat.Version97to2003);
            doc = null;
            //-------------------------------------

            CurrencyService.ClearData("Monetary");

            //-------------------------------------
            Workbook workbook = new Workbook();
            //workbook.LoadFromFile("https://uploadmvc.blob.core.windows.net/mvc/Upload/Monetary.xls");
            WebClient webClient1 = new WebClient();
            using (MemoryStream ms = new MemoryStream(webClient1.DownloadData("https://uploadmvc.blob.core.windows.net/mvc/Upload/Monetary.xls")))
            {
                workbook.LoadFromStream(ms);
            }
            Worksheet sheet = workbook.Worksheets[0];

            int Rows = sheet.LastRow;
            for (int i = Rows - 23; i < Rows + 1; i++)
            {
                MonetaryAggregate Temp_MA = new MonetaryAggregate();
                Temp_MA.QuotedDate = DateTime.Parse(sheet.Range["A" + i].Value);
                Temp_MA.M1A_Annual_Rate = Decimal.Parse(sheet.Range["C" + i].Value);
                Temp_MA.M1A_Daily = Decimal.Parse(sheet.Range["B" + i].Value);
                Temp_MA.M1B_Annual_Rate = Decimal.Parse(sheet.Range["G" + i].Value);
                Temp_MA.M1B_Daily = Decimal.Parse(sheet.Range["F" + i].Value);
                Temp_MA.M2_Annual_Rate = Decimal.Parse(sheet.Range["K" + i].Value);
                Temp_MA.M2_Daily = Decimal.Parse(sheet.Range["J" + i].Value);

                db.MonetaryAggregates.Add(Temp_MA);
            }
            db.SaveChanges();

            MemoryStream stram1 = new MemoryStream();
            workbook.SaveToStream(stram1, FileFormat.Version97to2003);
            xlsService.UploadXLSAzure("mvc", "Upload", "Taifex-" + "Monetary.xls", stram);
            //workbook.SaveToFile("https://uploadmvc.blob.core.windows.net/mvc/Upload/Monetary.xls", FileFormat.Version97to2003);

            sheet = null;
            workbook = null;

            return View();
        }

        public ActionResult TaifexXLS()
        {
            Taifex TaifexItem = new Taifex();
            TaifexItem = db.Taifexes.OrderByDescending(p => p.QuotedDate).Take(1).Single();

            Workbook workbook = new Workbook();
            //workbook.LoadFromFile(Server.MapPath("https://uploadmvc.blob.core.windows.net/mvc/Upload/Taifex.xls"));
            WebClient webClient = new WebClient();
            using (MemoryStream ms = new MemoryStream(webClient.DownloadData("https://uploadmvc.blob.core.windows.net/mvc/Upload/Taifex.xls")))
            {
                workbook.LoadFromStream(ms);
            }
            Worksheet sheet = workbook.Worksheets[0];
            //期貨
            sheet.Range["A2"].Value = "日期";
            sheet.Range["B2"].Value = "台股期貨";
            sheet.Range["C2"].Value = "多方";
            sheet.Range["D2"].Value = "增減";
            sheet.Range["E2"].Value = "空方";
            sheet.Range["F2"].Value = "增減";
            sheet.Range["G2"].Value = "多空淨額";
            sheet.Range["A3"].Value = TaifexItem.QuotedDate.ToString("yyyy/MM/dd");
            sheet.Range["B3"].Value = "自營商";
            sheet.Range["C3"].Value = TaifexItem.Future_Dealer_Buy.ToString();
            sheet.Range["E3"].Value = TaifexItem.Future_Dealer_Sell.ToString();
            sheet.Range["G3"].Value = TaifexItem.Future_Dealer_Net.ToString();
            sheet.Range["B4"].Value = "外資";
            sheet.Range["C4"].Value = TaifexItem.Future_Foreign_Buy.ToString();
            sheet.Range["E4"].Value = TaifexItem.Future_Foreign_Sell.ToString();
            sheet.Range["G4"].Value = TaifexItem.Future_Foreign_Net.ToString();
            sheet.Range["A7"].Value = "前五大";
            sheet.Range["A8"].Value = "多方";
            sheet.Range["B8"].Value = "部位";
            sheet.Range["C8"].Value = "空方";
            sheet.Range["D8"].Value = "部位";
            sheet.Range["A9"].Value = TaifexItem.Future_Top5_Buy.ToString();
            sheet.Range["B9"].Value = TaifexItem.Future_Top5_Buy_Percent.ToString("0.00") + "%";
            sheet.Range["C9"].Value = TaifexItem.Future_Top5_Sell.ToString();
            sheet.Range["D9"].Value = TaifexItem.Future_Top5_Sell_Percent.ToString("0.00") + "%";
            sheet.Range["A10"].Value = "前十大";
            sheet.Range["A11"].Value = "多方";
            sheet.Range["B11"].Value = "部位";
            sheet.Range["C11"].Value = "空方";
            sheet.Range["D11"].Value = "部位";
            sheet.Range["A12"].Value = TaifexItem.Future_Top10_Buy.ToString();
            sheet.Range["B12"].Value = TaifexItem.Future_Top10_Buy_Percent.ToString("0.00") + "%";
            sheet.Range["C12"].Value = TaifexItem.Future_Top10_Sell.ToString();
            sheet.Range["D12"].Value = TaifexItem.Future_Top10_Sell_Percent.ToString("0.00") + "%";
            //選擇權
            sheet.Range["J1"].Value = "選擇權契約";
            sheet.Range["K2"].Value = "多方";
            sheet.Range["L2"].Value = "增減";
            sheet.Range["M2"].Value = "空方";
            sheet.Range["N2"].Value = "增減";
            sheet.Range["O2"].Value = "多空淨額";
            sheet.Range["J3"].Value = "自營商";
            sheet.Range["K3"].Value = TaifexItem.Option_Dealer_Buy.ToString();
            sheet.Range["M3"].Value = TaifexItem.Option_Dealer_Sell.ToString();
            sheet.Range["O3"].Value = TaifexItem.Option_Dealer_Net.ToString();
            sheet.Range["J4"].Value = "外資";
            sheet.Range["K4"].Value = TaifexItem.Option_Foreign_Buy.ToString();
            sheet.Range["M4"].Value = TaifexItem.Option_Foreign_Sell.ToString();
            sheet.Range["O4"].Value = TaifexItem.Option_Foreign_Net.ToString();
            sheet.Range["J7"].Value = "前五大";
            sheet.Range["J8"].Value = "買方買權";
            sheet.Range["K8"].Value = "部位";
            sheet.Range["L8"].Value = "賣方賣權";
            sheet.Range["M8"].Value = "部位";
            sheet.Range["N8"].Value = "買方賣權";
            sheet.Range["O8"].Value = "部位";
            sheet.Range["P8"].Value = "賣方買權";
            sheet.Range["Q8"].Value = "部位";
            sheet.Range["J9"].Value = TaifexItem.Option_Top5_Long_Buy.ToString();
            sheet.Range["K9"].Value = TaifexItem.Option_Top5_Long_Buy_Percent.ToString("0.00") + "%";
            sheet.Range["L9"].Value = TaifexItem.Option_Top5_Short_Sell.ToString();
            sheet.Range["M9"].Value = TaifexItem.Option_Top5_Short_Sell_Percent.ToString("0.00") + "%";
            sheet.Range["N9"].Value = TaifexItem.Option_Top5_Long_Sell.ToString();
            sheet.Range["O9"].Value = TaifexItem.Option_Top5_Long_Sell_Percent.ToString("0.00") + "%";
            sheet.Range["P9"].Value = TaifexItem.Option_Top5_Short_Buy.ToString();
            sheet.Range["Q9"].Value = TaifexItem.Option_Top5_Short_Buy_Percent.ToString("0.00") + "%";
            sheet.Range["J10"].Value = "前十大";
            sheet.Range["J11"].Value = "買方買權";
            sheet.Range["K11"].Value = "部位";
            sheet.Range["L11"].Value = "賣方賣權";
            sheet.Range["M11"].Value = "部位";
            sheet.Range["N11"].Value = "買方賣權";
            sheet.Range["O11"].Value = "部位";
            sheet.Range["P11"].Value = "賣方買權";
            sheet.Range["Q11"].Value = "部位";
            sheet.Range["J12"].Value = TaifexItem.Option_Top10_Long_Buy.ToString();
            sheet.Range["K12"].Value = TaifexItem.Option_Top10_Long_Buy_Percent.ToString("0.00") + "%";
            sheet.Range["L12"].Value = TaifexItem.Option_Top10_Short_Sell.ToString();
            sheet.Range["M12"].Value = TaifexItem.Option_Top10_Short_Sell_Percent.ToString("0.00") + "%";
            sheet.Range["N12"].Value = TaifexItem.Option_Top10_Long_Sell.ToString();
            sheet.Range["O12"].Value = TaifexItem.Option_Top10_Long_Sell_Percent.ToString("0.00") + "%";
            sheet.Range["P12"].Value = TaifexItem.Option_Top10_Short_Buy.ToString();
            sheet.Range["Q12"].Value = TaifexItem.Option_Top10_Short_Buy_Percent.ToString("0.00") + "%";

            sheet.Range["A1"].Value = "期貨契約";

            MemoryStream stram = new MemoryStream();
            workbook.SaveToStream(stram, FileFormat.Version97to2003);
            xlsService.UploadXLSAzure("mvc", "Upload", "Taifex-" + TaifexItem.QuotedDate.ToString("yyyy.MM.dd") + ".xls", stram);

            //workbook.SaveToFile(Server.MapPath("https://uploadmvc.blob.core.windows.net/mvc/Upload/Taifex-" + TaifexItem.QuotedDate.ToString("yyyy.MM.dd") + ".xls"), FileFormat.Version97to2003);

            sheet = null;
            workbook = null;
            TempData["Date"] = TaifexItem.QuotedDate.ToString("yyyy.MM.dd");
            return RedirectToAction("DownloadTaifexExcel");
        }

        public ActionResult DownloadTaifexExcel()
        {
            /*  string Path = Server.MapPath("https://uploadmvc.blob.core.windows.net/mvc/Upload/Taifex-" + TempData["Date"].ToString() + ".xls");
              Stream iStream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
              return File(iStream, "application/ms-excel", "Taifex-" + TempData["Date"].ToString() + ".xls");*/

            string URL = "https://uploadmvc.blob.core.windows.net/mvc/Upload/Taifex-" + TempData["Date"].ToString() + ".xls";
            WebClient webClient = new WebClient();
            MemoryStream ms = new MemoryStream(webClient.DownloadData(URL));
            return File(ms, "application/vnd.ms-excel", URL.Substring(URL.LastIndexOf("/")));
        }
    }
}