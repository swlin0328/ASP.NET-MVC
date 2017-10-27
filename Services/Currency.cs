using WebApplication3.Models;
using System.IO;
using System.Net;
using Spire.Xls;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using System.Text;
using ScrapySharp.Extensions;

namespace WebApplication3.Services
{
    public class Currency : Controller
    {
        FinanceEntities db = new FinanceEntities();
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void ClearData(string cmd)
        {
            try
            {
                if (cmd == "Monetary")
                {
                    IQueryable<MonetaryAggregate> tempDataList = db.MonetaryAggregates;
                    List<MonetaryAggregate> MonetaryItems = tempDataList.ToList();
                    foreach (var item in MonetaryItems)
                    {
                        db.MonetaryAggregates.Remove(item);
                    }
                }
                else if (cmd == "Libor")
                {
                    IQueryable<Libor> tempDataList = db.Libors;
                    List<Libor> LiborItems = tempDataList.ToList();
                    foreach (var item in LiborItems)
                    {
                        db.Libors.Remove(item);
                    }
                }
                db.SaveChanges();
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return;
            }
        }

        public List<MonetaryAggregate> getMonetaryList()
        {
            try
            {
                IQueryable<MonetaryAggregate> SearchData;
                SearchData = db.MonetaryAggregates;

                return SearchData.OrderByDescending(p => p.QuotedDate).ToList();
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }


        public List<Libor> getLiborList()
        {
            try
            {
                IQueryable<Libor> SearchData;
                SearchData = db.Libors;

                return SearchData.OrderByDescending(p => p.QuotedDate).ToList();
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public void UpdateLibor()
        {
            try
            {
                ClearData("Libor");
                Libor LiborItem = new Libor();
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("https://ibank.firstbank.com.tw/NetBank/7/0117.html?sh=none"));
                //以GoogleFinance為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();

                HtmlNode tempNode;
                List<HtmlNode> trNode;
                doc.Load(ms, Encoding.Default);

                tempNode = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/table[2]");
                trNode = tempNode.CssSelect("tr").ToList();

                for (int i = 3; i < 180; i++)
                {
                    Libor TempLibor = new Libor();

                    TempLibor.QuotedDate = DateTime.Parse(trNode[i].SelectSingleNode(@"td[1]").InnerText);
                    TempLibor.C0N = Decimal.Parse(trNode[i].SelectSingleNode(@"td[2]").InnerText);
                    TempLibor.C1W = Decimal.Parse(trNode[i].SelectSingleNode(@"td[3]").InnerText);
                    TempLibor.C1M = Decimal.Parse(trNode[i].SelectSingleNode(@"td[4]").InnerText);
                    TempLibor.C3M = Decimal.Parse(trNode[i].SelectSingleNode(@"td[5]").InnerText);
                    TempLibor.C6M = Decimal.Parse(trNode[i].SelectSingleNode(@"td[6]").InnerText);
                    TempLibor.C12M = Decimal.Parse(trNode[i].SelectSingleNode(@"td[7]").InnerText);

                    db.Libors.Add(TempLibor);
                }
                db.SaveChanges();

                //清除資料
                doc = null;
                url = null;
                ms.Close();
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return;
            }
        }

        public Taifex UpdateTaifex()
        {
            try
            {
                Taifex TaifexItem = new Taifex();

                TaifexItem = Future(TaifexItem);
                TaifexItem = Option(TaifexItem);
                TaifexItem = Future_Top10(TaifexItem);
                TaifexItem = Option_Top10(TaifexItem);

                db.Taifexes.Add(TaifexItem);
                db.SaveChanges();

                return TaifexItem;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public Taifex Future(Taifex TaifexItem)
        {
            try
            {
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("http://www.taifex.com.tw/chinese/3/7_12_3_tbl.asp"));
                //以GoogleFinance為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();

                HtmlNode tempNode;
                doc.Load(ms, Encoding.UTF8);
                tempNode = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[1]/div[1]/div[2]/table[1]/tbody[1]/tr[2]/td[1]/table[1]/tbody[1]");

                //台指契約
                TaifexItem.QuotedDate = DateTime.Parse(doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[1]/div[1]/div[2]/table[1]/tbody[1]/tr[1]/td[1]/p[1]/span[2]").InnerText.ToString().Replace("日期", ""));
                TaifexItem.Future_Dealer_Buy = Decimal.Parse(tempNode.SelectSingleNode(@"tr[4]/td[10]").InnerText.Replace(",", "").ToString());
                TaifexItem.Future_Dealer_Sell = Decimal.Parse(tempNode.SelectSingleNode(@"tr[4]/td[12]").InnerText.Replace(",", "").ToString());
                TaifexItem.Future_Dealer_Net = Decimal.Parse(tempNode.SelectSingleNode(@"tr[4]/td[14]").InnerText.Replace(",", "").ToString());
                TaifexItem.Future_Foreign_Buy = Decimal.Parse(tempNode.SelectSingleNode(@"tr[6]/td[8]").InnerText.Replace(",", "").ToString());
                TaifexItem.Future_Foreign_Sell = Decimal.Parse(tempNode.SelectSingleNode(@"tr[6]/td[10]").InnerText.Replace(",", "").ToString());
                TaifexItem.Future_Foreign_Net = Decimal.Parse(tempNode.SelectSingleNode(@"tr[6]/td[12]").InnerText.Replace(",", "").ToString());

                doc = null;
                url = null;
                ms.Close();

                return TaifexItem;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public Taifex Future_Top10(Taifex TaifexItem)
        {
            try
            {
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("http://www.taifex.com.tw/chinese/3/7_8_tbl.asp"));
                //以GoogleFinance為範例


                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                HtmlNode tempNode;
                string Text;

                doc.Load(ms, Encoding.UTF8);
                tempNode = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[3]/table[1]/tr[2]/td[1]/table[1]");
                //期貨前十大
                Text = tempNode.SelectSingleNode(@"tr[5]/td[2]").InnerText;
                TaifexItem.Future_Top5_Buy = Decimal.Parse(Text.Remove(Text.IndexOf("(")).Replace(",", "").Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[5]/td[3]").InnerText;
                TaifexItem.Future_Top5_Buy_Percent = Decimal.Parse(Text.Remove(Text.IndexOf("%")).Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[5]/td[6]").InnerText;
                TaifexItem.Future_Top5_Sell = Decimal.Parse(Text.Remove(Text.IndexOf("(")).Replace(",", "").Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[5]/td[7]").InnerText;
                TaifexItem.Future_Top5_Sell_Percent = Decimal.Parse(Text.Remove(Text.IndexOf("%")).Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[5]/td[4]").InnerText;
                TaifexItem.Future_Top10_Buy = Decimal.Parse(Text.Remove(Text.IndexOf("(")).Replace(",", "").Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[5]/td[5]").InnerText;
                TaifexItem.Future_Top10_Buy_Percent = Decimal.Parse(Text.Remove(Text.IndexOf("%")).Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[5]/td[8]").InnerText;
                TaifexItem.Future_Top10_Sell = Decimal.Parse(Text.Remove(Text.IndexOf("(")).Replace(",", "").Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[5]/td[9]").InnerText;
                TaifexItem.Future_Top10_Sell_Percent = Decimal.Parse(Text.Remove(Text.IndexOf("%")).Trim().ToString());

                doc = null;
                url = null;
                ms.Close();
                return TaifexItem;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public Taifex Option(Taifex TaifexItem)
        {
            try
            {
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("http://www.taifex.com.tw/chinese/3/7_12_4_tbl.asp"));
                //以GoogleFinance為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                HtmlNode tempNode;

                doc.Load(ms, Encoding.UTF8);
                tempNode = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[1]/div[1]/div[2]/table[1]/tbody[1]/tr[2]/td[1]/table[1]/tbody[1]");
                //選擇權契約
                TaifexItem.Option_Dealer_Buy = Decimal.Parse(tempNode.SelectSingleNode(@"tr[4]/td[10]").InnerText.Replace(",", "").ToString());
                TaifexItem.Option_Dealer_Sell = Decimal.Parse(tempNode.SelectSingleNode(@"tr[4]/td[12]").InnerText.Replace(",", "").ToString());
                TaifexItem.Option_Dealer_Net = Decimal.Parse(tempNode.SelectSingleNode(@"tr[4]/td[14]").InnerText.Replace(",", "").ToString());
                TaifexItem.Option_Foreign_Buy = Decimal.Parse(tempNode.SelectSingleNode(@"tr[6]/td[8]").InnerText.Replace(",", "").ToString());
                TaifexItem.Option_Foreign_Sell = Decimal.Parse(tempNode.SelectSingleNode(@"tr[6]/td[10]").InnerText.Replace(",", "").ToString());
                TaifexItem.Option_Foreign_Net = Decimal.Parse(tempNode.SelectSingleNode(@"tr[6]/td[12]").InnerText.Replace(",", "").ToString());

                doc = null;
                url = null;
                ms.Close();
                return TaifexItem;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public Taifex Option_Top10(Taifex TaifexItem)
        {
            try
            {
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("http://www.taifex.com.tw/chinese/3/7_9_tbl.asp"));
                //以GoogleFinance為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                HtmlNode tempNode;
                string Text;

                doc.Load(ms, Encoding.UTF8);
                tempNode = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[3]/table[1]/tr[2]/td[1]/table[1]");
                //買權
                Text = tempNode.SelectSingleNode(@"tr[5]/td[2]").InnerText;
                TaifexItem.Option_Top5_Long_Buy = Decimal.Parse(Text.Remove(Text.IndexOf("(")).Replace(",", "").Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[5]/td[3]").InnerText;
                TaifexItem.Option_Top5_Long_Buy_Percent = Decimal.Parse(Text.Remove(Text.IndexOf("%")).Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[5]/td[6]").InnerText;
                TaifexItem.Option_Top5_Short_Buy = Decimal.Parse(Text.Remove(Text.IndexOf("(")).Replace(",", "").Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[5]/td[7]").InnerText;
                TaifexItem.Option_Top5_Short_Buy_Percent = Decimal.Parse(Text.Remove(Text.IndexOf("%")).Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[5]/td[4]").InnerText;
                TaifexItem.Option_Top10_Long_Buy = Decimal.Parse(Text.Remove(Text.IndexOf("(")).Replace(",", "").Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[5]/td[5]").InnerText;
                TaifexItem.Option_Top10_Long_Buy_Percent = Decimal.Parse(Text.Remove(Text.IndexOf("%")).Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[5]/td[8]").InnerText;
                TaifexItem.Option_Top10_Short_Buy = Decimal.Parse(Text.Remove(Text.IndexOf("(")).Replace(",", "").Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[5]/td[9]").InnerText;
                TaifexItem.Option_Top10_Short_Buy_Percent = Decimal.Parse(Text.Remove(Text.IndexOf("%")).Trim().ToString());
                //賣權
                Text = tempNode.SelectSingleNode(@"tr[8]/td[2]").InnerText;
                TaifexItem.Option_Top5_Long_Sell = Decimal.Parse(Text.Remove(Text.IndexOf("(")).Replace(",", "").Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[8]/td[3]").InnerText;
                TaifexItem.Option_Top5_Long_Sell_Percent = Decimal.Parse(Text.Remove(Text.IndexOf("%")).Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[8]/td[6]").InnerText;
                TaifexItem.Option_Top5_Short_Sell = Decimal.Parse(Text.Remove(Text.IndexOf("(")).Replace(",", "").Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[8]/td[7]").InnerText;
                TaifexItem.Option_Top5_Short_Sell_Percent = Decimal.Parse(Text.Remove(Text.IndexOf("%")).Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[8]/td[4]").InnerText;
                TaifexItem.Option_Top10_Long_Sell = Decimal.Parse(Text.Remove(Text.IndexOf("(")).Replace(",", "").Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[8]/td[5]").InnerText;
                TaifexItem.Option_Top10_Long_Sell_Percent = Decimal.Parse(Text.Remove(Text.IndexOf("%")).Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[8]/td[8]").InnerText;
                TaifexItem.Option_Top10_Short_Sell = Decimal.Parse(Text.Remove(Text.IndexOf("(")).Replace(",", "").Trim().ToString());
                Text = tempNode.SelectSingleNode(@"tr[8]/td[9]").InnerText;
                TaifexItem.Option_Top10_Short_Sell_Percent = Decimal.Parse(Text.Remove(Text.IndexOf("%")).Trim().ToString());
                //清除資料
                doc = null;
                url = null;
                ms.Close();
                return TaifexItem;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }
    }
}