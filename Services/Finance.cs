using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;
using System.IO;
using HtmlAgilityPack;
using System.Text;
using System.Net;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using WebApplication3.ViewModels;

namespace WebApplication3.Services
{
    public class Finance
    {
        FinanceEntities db = new FinanceEntities();
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public StockItem getStock(string Num)
        {
            try
            {
                StockItem stock = new StockItem();
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("https://tw.stock.yahoo.com/q/q?s=" + Num.ToString()));
                //以GoogleFinance為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                HtmlNode tempNode;
                List<HtmlNode> tdNode;
                string Text1, Text2;
                doc.Load(ms, Encoding.GetEncoding("big5"));

                tempNode = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/center[1]/table[2]/tr[1]/td[1]/table[1]/tr[2]");
                // 取得股價 
                stock.Name = tempNode.SelectSingleNode(@"td[1]").InnerText.ToString();
                if (stock.Name.IndexOf("加") > 0)
                {
                    stock.Name = stock.Name.Remove(stock.Name.IndexOf("加"));
                }
                stock.Time = tempNode.SelectSingleNode(@"td[2]").InnerText.ToString();
                stock.Deal = Decimal.Parse(tempNode.SelectSingleNode(@"td[3]").InnerText.ToString());
                if (tempNode.SelectSingleNode(@"td[4]").InnerText.ToString() == "－")
                {
                    stock.Buy = Decimal.Parse(tempNode.SelectSingleNode(@"td[5]").InnerText.ToString());
                }
                else
                {
                    stock.Buy = Decimal.Parse(tempNode.SelectSingleNode(@"td[4]").InnerText.ToString());
                }
                if (tempNode.SelectSingleNode(@"td[5]").InnerText.ToString() == "－")
                {
                    stock.Sell = Decimal.Parse(tempNode.SelectSingleNode(@"td[4]").InnerText.ToString());
                }
                else
                {
                    stock.Sell = Decimal.Parse(tempNode.SelectSingleNode(@"td[5]").InnerText.ToString());
                }

                tdNode = tempNode.CssSelect("td").ToList();

                Text1 = tdNode[5].InnerText.ToString();
                Text2 = tdNode[6].InnerText.ToString();
                stock.Change = Text1.Remove(Text1.IndexOf(Text2)).Trim();

                stock.Volume = int.Parse(tdNode[6].InnerText.ToString().Replace(",", "").Trim());
                stock.lastDay = Decimal.Parse(tdNode[7].InnerText.ToString().Trim());
                stock.Open = Decimal.Parse(tdNode[8].InnerText.ToString().Trim());
                stock.High = Decimal.Parse(tdNode[9].InnerText.ToString().Trim());
                stock.Low = Decimal.Parse(tdNode[10].InnerText.ToString().Trim());

                //清除資料
                doc = null;
                url = null;
                ms.Close();

                return stock;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public List<Stock> getStockList(ForPaging Paging, string Num)
        {
            try
            {
                IQueryable<Stock> SearchData;
                bool numIsEmpty = String.IsNullOrEmpty(Num);

                if (!numIsEmpty)
                {
                    int Id = int.Parse(Num);
                    SearchData = db.Stocks.Where(p => p.Id.Equals(Id));

                    if (SearchData.Count() == 0)
                    {
                        List<Stock> nullStock = new List<Stock>();
                        Paging.MaxPage = 1;
                        Paging.SetRightPage();
                        return nullStock;
                    }
                    else
                    {
                        Paging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(SearchData.Count() / Paging.ItemNum)));
                        Paging.SetRightPage();
                        return SearchData.ToList().OrderByDescending(p => p.Price_Date).Skip((Paging.NowPage - 1) * Paging.ItemNum).Take(Paging.ItemNum).ToList();
                    }
                }
                else
                {
                    List<Stock> nullStock = new List<Stock>();
                    Paging.MaxPage = 1;
                    Paging.SetRightPage();
                    return nullStock;
                }
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public List<Exchange> getExchangeList(ForPaging Paging, string Currency)
        {
            try
            {
                IQueryable<Exchange> SearchData;
                SearchData = db.Exchanges.Where(p => p.Currency.Equals(Currency));

                if (String.IsNullOrEmpty(Currency) || SearchData.Count() == 0)
                {
                    List<Exchange> nullExchange = new List<Exchange>();
                    Paging.MaxPage = 1;
                    Paging.SetRightPage();
                    return nullExchange;
                }
                else
                {
                    Paging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(SearchData.Count() / Paging.ItemNum)));
                    Paging.SetRightPage();
                    return SearchData.OrderByDescending(p => p.QuotedDate).Skip((Paging.NowPage - 1) * Paging.ItemNum).Take(Paging.ItemNum).ToList();
                }
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public void UpdateStock(int Num)
        {
            try
            {
                List<Stock> stockList = new List<Stock>();
                int nodes;
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("https://www.google.com/finance/historical?q=TPE%3A" + Num + "&start=0&num=220"));
                //以GoogleFinance為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                List<HtmlNode> tdNode;
                HtmlNode tempNode;
                string Text0;
                doc.Load(ms, Encoding.Default);

                Text0 = ((doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]").CssSelect("#gf-viewc").ToList())[0].CssSelect("h3").ToList())[0].InnerHtml.ToString();
                tempNode = (doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]").CssSelect("#prices").ToList())[0];
                tdNode = tempNode.CssSelect("td").ToList();
                nodes = (tempNode.CssSelect("tr").Count())/2;

                // 取得股價 
                for (int num = 0; num < nodes; num++)
                {
                    string Text1, Text2;
                    Stock stock_temp = new Stock();
                    stock_temp.Id = Num;

                    if (Text0.IndexOf("Co.") > 0)
                    {
                        stock_temp.stockName = Text0.Remove(Text0.IndexOf("Co.")).Trim();
                    }
                    else
                    {
                        stock_temp.stockName = Text0.Trim();
                    }

                    Text1 = tdNode[0 + 6 * num].InnerText;
                    Text2 = tdNode[1 + 6 * num].InnerText;
                    stock_temp.Price_Date = DateTime.Parse(Text(Text1, Text2));

                    Text1 = tdNode[1 + 6 * num].InnerText;
                    Text2 = tdNode[2 + 6 * num].InnerText;
                    stock_temp.Price_Open = Decimal.Parse(Text(Text1, Text2));

                    Text1 = tdNode[2 + 6 * num].InnerText;
                    Text2 = tdNode[3 + 6 * num].InnerText;
                    stock_temp.Price_High = Decimal.Parse(Text(Text1, Text2));

                    Text1 = tdNode[3 + 6 * num].InnerText;
                    Text2 = tdNode[4 + 6 * num].InnerText;
                    stock_temp.Price_Low = Decimal.Parse(Text(Text1, Text2));

                    Text1 = tdNode[4 + 6 * num].InnerText;
                    Text2 = tdNode[5 + 6 * num].InnerText;
                    stock_temp.Price_Close = Decimal.Parse(Text(Text1, Text2));

                    Text1 = tdNode[5 + 6 * num].InnerText;
                    Text2 = tdNode[6 + 6 * num].InnerText;
                    stock_temp.Volume = Int32.Parse(Text(Text1, Text2));

                    stockList.Add(stock_temp);
                }

                foreach (var item in stockList)
                {
                    db.Stocks.Add(item);
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

        public string Text(string stockText1, string stockText2)
        {
            try
            {
                string stockText = stockText1.Replace(stockText2, "").Replace(",", "").Trim();
                return stockText;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public List<Exchange> getExchange()
        {
            try
            {
                List<Exchange> exchangeList = new List<Exchange>();
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("http://rate.bot.com.tw/xrt?Lang=zh-TW"));
                //以台灣銀行為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                doc.Load(ms, Encoding.UTF8);
                HtmlNode tempNode;

                tempNode = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/main[1]/div[4]/table[1]/tbody[1]");

                // 取得匯率 
                for (int num = 1; num < 9; num++)
                {
                    Exchange exchange_Temp1 = new Exchange();

                    exchange_Temp1.Currency = tempNode.SelectSingleNode(@"tr[" + num + "]/td[1]/div[1]/div[2]").InnerText;
                    exchange_Temp1.QuotedDate = DateTime.Parse(DateTime.Now.AddHours(8).ToString("yyyy/MM/dd hh:mm"));
                    exchange_Temp1.CashRate_Buying = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + num + "]/td[2]").InnerText.ToString());
                    exchange_Temp1.CashRate_Selling = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + num + "]/td[3]").InnerText.ToString());
                    exchange_Temp1.SpotRate_Buying = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + num + "]/td[4]").InnerText.ToString());
                    exchange_Temp1.SpotRate_Selling = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + num + "]/td[5]").InnerText.ToString());

                    exchangeList.Add(exchange_Temp1);
                }

                Exchange exchange_Temp = new Exchange();

                exchange_Temp.Currency = tempNode.SelectSingleNode(@"tr[15]/td[1]/div[1]/div[2]").InnerText;
                exchange_Temp.QuotedDate = DateTime.Parse(DateTime.Now.AddHours(8).ToString("yyyy/MM/dd hh:mm"));
                exchange_Temp.CashRate_Buying = Decimal.Parse(tempNode.SelectSingleNode(@"tr[15]/td[2]").InnerText.ToString());
                exchange_Temp.CashRate_Selling = Decimal.Parse(tempNode.SelectSingleNode(@"tr[15]/td[3]").InnerText.ToString());
                exchange_Temp.SpotRate_Buying = Decimal.Parse(tempNode.SelectSingleNode(@"tr[15]/td[4]").InnerText.ToString());
                exchange_Temp.SpotRate_Selling = Decimal.Parse(tempNode.SelectSingleNode(@"tr[15]/td[5]").InnerText.ToString());

                exchangeList.Add(exchange_Temp);

                //清除資料
                doc = null;
                url = null;
                ms.Close();

                return exchangeList;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public void UpdateExchange(string Currency)
        {
            try
            {
                List<Exchange> exchangeList = new List<Exchange>();
                int nodes;
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("http://rate.bot.com.tw/xrt/quote/l6m/" + Currency + "?Lang=zh-TW"));
                //以台灣銀行為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                doc.Load(ms, Encoding.UTF8);
                HtmlNode tempNode;

                tempNode = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/main[1]/div[4]/table[1]/tbody[1]");
                nodes = tempNode.SelectNodes("tr").Count();

                // 取得匯率 
                for (int num = 1; num < nodes; num++)
                {
                    Exchange exchange_Temp = new Exchange();

                    exchange_Temp.Currency = Currency.ToUpper();
                    exchange_Temp.QuotedDate = DateTime.Parse(tempNode.SelectSingleNode(@"tr[" + num + "]/td[1]").InnerText.ToString());
                    exchange_Temp.CashRate_Buying = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + num + "]/td[3]").InnerText.ToString());
                    exchange_Temp.CashRate_Selling = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + num + "]/td[4]").InnerText.ToString());
                    exchange_Temp.SpotRate_Buying = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + num + "]/td[5]").InnerText.ToString());
                    exchange_Temp.SpotRate_Selling = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + num + "]/td[6]").InnerText.ToString());

                    exchangeList.Add(exchange_Temp);
                }

                foreach (var item in exchangeList)
                {
                    db.Exchanges.Add(item);
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

        public void clearData(string dataType, string Currency, int Id)
        {
            try
            {
                if (dataType == "Stock")
                {
                    IQueryable<Stock> tempDataList = db.Stocks.Where(p => p.Id.Equals(Id));
                    List<Stock> stockItems = tempDataList.ToList();
                    foreach (var item in stockItems)
                    {
                        db.Stocks.Remove(item);
                    }
                    db.SaveChanges();
                }
                else if (dataType == "Exchange")
                {
                    IQueryable<Exchange> tempDataList = db.Exchanges.Where(p => p.Currency.Equals(Currency));
                    List<Exchange> exchangeItems = tempDataList.ToList();
                    foreach (var item in exchangeItems)
                    {
                        db.Exchanges.Remove(item);
                    }
                    db.SaveChanges();
                }
                else if (dataType == "Economic")
                {
                    IQueryable<Macroeconomic> tempDataList = db.Macroeconomics;
                    List<Macroeconomic> EconomicItems = tempDataList.ToList();
                    foreach (var item in EconomicItems)
                    {
                        db.Macroeconomics.Remove(item);
                    }
                    db.SaveChanges();
                }
                else if (dataType == "CYQ")
                {
                    IQueryable<CYQ> tempDataList = db.CYQs.Where(p => p.Stock_Number.Equals(Id));
                    List<CYQ> CYQItems = tempDataList.ToList();
                    foreach (var item in CYQItems)
                    {
                        db.CYQs.Remove(item);
                    }
                    db.SaveChanges();
                }
                else if (dataType == "Voucher")
                {
                    IQueryable<Voucher> tempDataList = db.Vouchers.Where(p => p.Stock_Number.Equals(Id));
                    List<Voucher> VoucherItems = tempDataList.ToList();
                    foreach (var item in VoucherItems)
                    {
                        db.Vouchers.Remove(item);
                    }
                    db.SaveChanges();
                }
                else if (dataType == "BDI")
                {
                    IQueryable<CRB_BDI> tempDataList = db.CRB_BDI;
                    List<CRB_BDI> CRB_BDIItems = tempDataList.ToList();
                    foreach (var item in CRB_BDIItems)
                    {
                        db.CRB_BDI.Remove(item);
                    }
                    db.SaveChanges();
                }
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return;
            }
        }

        public void autoStockRefresh()
        {
            try
            {
                List<int> stockNumber = (db.Stocks.Select(p => p.Id)).Distinct().ToList();
                foreach (var item in stockNumber)
                {
                    clearData("Stock", "", item);
                    UpdateStock(item);
                }
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return;
            }
        }

        public void autoExchangeRefresh()
        {
            try
            {
                List<string> Currency = (db.Exchanges.Select(p => p.Currency.ToString())).Distinct().ToList();
                foreach (var item in Currency)
                {
                    clearData("Exchange", item, 0);
                    UpdateExchange(item);
                }
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return;
            }
        }

        public void autoCYQRefresh()
        {
            try
            {
                List<int> stockNumber = (db.Stocks.Select(p => p.Id)).Distinct().ToList();
                foreach (var item in stockNumber)
                {
                    clearData("CYQ", "", item);
                    UpdateCYQ(item);
                }
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return;
            }
        }

        public void autoVoucherRefresh()
        {
            try
            {
                List<int> stockNumber = (db.Stocks.Select(p => p.Id)).Distinct().ToList();
                foreach (var item in stockNumber)
                {
                    clearData("Voucher", "", item);
                    UpdateVouchers(item);
                }
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return;
            }
        }

        public List<RateItem> getRate()
        {
            try
            {
                List<RateItem> RateList = new List<RateItem>();

                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("http://www.cnyes.com/CentralBank/interest1.aspx?type=all"));
                //以台灣銀行為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                doc.Load(ms, Encoding.UTF8);
                HtmlNode tempNode;
                string[] RateOrder = new string[] { "1", "2", "3", "6", "7", "13", "15", "16", "17", "18", "19", "23" };

                tempNode = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[6]/div[1]/div[1]/div[2]/div[2]/table[1]");

                // 取得匯率 
                for (int num = 0; num < RateOrder.Count(); num++)
                {
                    RateItem Rate = new RateItem();

                    Rate.Country = tempNode.SelectSingleNode(@"tr[" + RateOrder[num] + "]/td[1]").InnerText;
                    Rate.RateName = tempNode.SelectSingleNode(@"tr[" + RateOrder[num] + "]/td[2]").InnerText;
                    Rate.Rate = tempNode.SelectSingleNode(@"tr[" + RateOrder[num] + "]/td[3]").InnerText;
                    Rate.LastRate = tempNode.SelectSingleNode(@"tr[" + RateOrder[num] + "]/td[6]").InnerText;
                    Rate.point = tempNode.SelectSingleNode(@"tr[" + RateOrder[num] + "]/td[4]").InnerText;
                    Rate.pubDate = tempNode.SelectSingleNode(@"tr[" + RateOrder[num] + "]/td[5]").InnerText;
                    Rate.Inflation = tempNode.SelectSingleNode(@"tr[" + RateOrder[num] + "]/td[7]").InnerText;

                    RateList.Add(Rate);
                }

                //清除資料
                doc = null;
                url = null;
                ms.Close();

                return RateList;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public void UpdateMacroEconomic()
        {
            try
            {
                clearData("Economic", "", 0);

                List<Macroeconomic> MacroeconomicList = new List<Macroeconomic>();

                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("http://www.cnyes.com/economy/indicator/EconomicalFollow/Usa_Major.aspx?id=4"));
                //以台灣銀行為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                doc.Load(ms, Encoding.UTF8);
                HtmlNode tempNode;

                tempNode = doc.DocumentNode.CssSelect("#container").ToList()[0];
                tempNode = tempNode.SelectSingleNode(@"div[2]/div[2]/div[2]//div[1]/table[1]");

                // 取得匯率 
                for (int i = 3; i < 10; i++)
                {
                    Macroeconomic Index = new Macroeconomic();

                    Index.Publish_Date = DateTime.Parse(tempNode.SelectSingleNode(@"tr[1]/th[" + i.ToString() + "]").InnerText);
                    Index.CPI = Decimal.Parse(tempNode.SelectSingleNode(@"tr[2]/td[" + i.ToString() + "]").InnerText);
                    Index.CPI_GrowthRate = Decimal.Parse(tempNode.SelectSingleNode(@"tr[4]/td[" + i.ToString() + "]").InnerText);
                    Index.CCPI = Decimal.Parse(tempNode.SelectSingleNode(@"tr[5]/td[" + i.ToString() + "]").InnerText);
                    Index.CCPI_GrowthRate = Decimal.Parse(tempNode.SelectSingleNode(@"tr[7]/td[" + i.ToString() + "]").InnerText);
                    Index.PPI_GrowthRate = Decimal.Parse(tempNode.SelectSingleNode(@"tr[8]/td[" + i.ToString() + "]").InnerText);
                    Index.Unemployment_Rate = Decimal.Parse(tempNode.SelectSingleNode(@"tr[9]/td[" + i.ToString() + "]").InnerText);
                    Index.NFP = int.Parse(tempNode.SelectSingleNode(@"tr[10]/td[" + i.ToString() + "]").InnerText);
                    Index.Weekly_Work_Hours = Decimal.Parse(tempNode.SelectSingleNode(@"tr[11]/td[" + i.ToString() + "]").InnerText);
                    Index.Hourly_WageRate = Decimal.Parse(tempNode.SelectSingleNode(@"tr[14]/td[" + i.ToString() + "]").InnerText);
                    Index.Export_IncreasedRate = Decimal.Parse(tempNode.SelectSingleNode(@"tr[23]/td[" + i.ToString() + "]").InnerText);
                    Index.Export_PriceRate = Decimal.Parse(tempNode.SelectSingleNode(@"tr[29]/td[" + i.ToString() + "]").InnerText);
                    Index.Import_IncreasedRate = Decimal.Parse(tempNode.SelectSingleNode(@"tr[25]/td[" + i.ToString() + "]").InnerText);
                    Index.Import_PriceRate = Decimal.Parse(tempNode.SelectSingleNode(@"tr[27]/td[" + i.ToString() + "]").InnerText);
                    Index.Housing_Starts = Decimal.Parse(tempNode.SelectSingleNode(@"tr[30]/td[" + i.ToString() + "]").InnerText);
                    Index.Housing_Starts_GrowthRate = Decimal.Parse(tempNode.SelectSingleNode(@"tr[32]/td[" + i.ToString() + "]").InnerText);
                    Index.Retail_Sales_Rate = Decimal.Parse(tempNode.SelectSingleNode(@"tr[50]/td[" + i.ToString() + "]").InnerText);
                    Index.Retail_Sales_Rate_NoCar = Decimal.Parse(tempNode.SelectSingleNode(@"tr[52]/td[" + i.ToString() + "]").InnerText);
                    Index.Personal_Expenditure_Rate = Decimal.Parse(tempNode.SelectSingleNode(@"tr[54]/td[" + i.ToString() + "]").InnerText);
                    Index.Individual_Income_Rate = Decimal.Parse(tempNode.SelectSingleNode(@"tr[56]/td[" + i.ToString() + "]").InnerText);
                    Index.ICS = Decimal.Parse(tempNode.SelectSingleNode(@"tr[63]/td[" + i.ToString() + "]").InnerText);
                    Index.Capacity_Utilization = Decimal.Parse(tempNode.SelectSingleNode(@"tr[66]/td[" + i.ToString() + "]").InnerText);
                    Index.Durable_Orders = Decimal.Parse(tempNode.SelectSingleNode(@"tr[70]/td[" + i.ToString() + "]").InnerText);
                    Index.Factory_Orders = Decimal.Parse(tempNode.SelectSingleNode(@"tr[74]/td[" + i.ToString() + "]").InnerText);
                    Index.Factory_Stock = Decimal.Parse(tempNode.SelectSingleNode(@"tr[76]/td[" + i.ToString() + "]").InnerText);
                    Index.Factory_Shipments = Decimal.Parse(tempNode.SelectSingleNode(@"tr[78]/td[" + i.ToString() + "]").InnerText);
                    Index.Manufacturing_PMI = Decimal.Parse(tempNode.SelectSingleNode(@"tr[87]/td[" + i.ToString() + "]").InnerText);
                    Index.PMI = Decimal.Parse(tempNode.SelectSingleNode(@"tr[89]/td[" + i.ToString() + "]").InnerText);

                    MacroeconomicList.Add(Index);
                }

                foreach (var item in MacroeconomicList)
                {
                    db.Macroeconomics.Add(item);
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

        public List<Macroeconomic> getEconomic()
        {
            try
            {
                IQueryable<Macroeconomic> SearchData = db.Macroeconomics;
                return SearchData.ToList().OrderByDescending(p => p.Publish_Date).ToList();
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public void UpdateBDI_CRB()
        {
            try
            {
                clearData("BDI", "", 0);
                CRB_BDI[] CRBList = new CRB_BDI[20];
                for (int i = 0; i < 20; i++)
                {
                    CRBList[i] = new CRB_BDI();
                }
                UpdateBDI(CRBList);
                UpdateCRB(CRBList);
                UpdateOil(CRBList);

                foreach (var item in CRBList)
                {
                    db.CRB_BDI.Add(item);
                }
                db.SaveChanges();

            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return;
            }
        }

        public void UpdateBDI(CRB_BDI[] CRBList)
        {
            try
            {
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("http://www.stockq.org/index/BDI.php"));
                //以台灣銀行為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                doc.Load(ms, Encoding.UTF8);
                HtmlNode tempNode;

                tempNode = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/table[1]/tr[1]/td[2]/center[5]/table[1]/center[1]/table[1]");

                // 取得匯率 
                for (int i = 2; i < 12; i++)
                {
                    CRBList[i-2].Publish_Date = DateTime.Parse(tempNode.SelectSingleNode(@"tr[" + i.ToString() +"]/td[1]").InnerText);
                    CRBList[i-2].BDI = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + i.ToString() + "]/td[2]").InnerText);
                    CRBList[i+8].Publish_Date = DateTime.Parse(tempNode.SelectSingleNode(@"tr[" + i.ToString() + "]/td[4]").InnerText);
                    CRBList[i+8].BDI = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + i.ToString() + "]/td[5]").InnerText);
                }

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

        public void UpdateCRB(CRB_BDI[] CRBList)
        {
            try
            {
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("http://www.stockq.org/index/CRB.php"));
                //以台灣銀行為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                doc.Load(ms, Encoding.UTF8);
                HtmlNode tempNode;

                tempNode = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/table[1]/tr[1]/td[2]/center[5]/table[1]/center[1]/table[1]");

                // 取得匯率 
                for (int i = 2; i < 12; i++)
                {
                    CRBList[i-2].CRB = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + i.ToString() + "]/td[2]").InnerText);
                    CRBList[i+8].CRB = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + i.ToString() + "]/td[5]").InnerText);
                }

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

        public void UpdateOil(CRB_BDI[] CRBList)
        {
            try
            {
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("http://www.stockq.org/commodity/FUTRWOIL.php"));
                //以台灣銀行為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                doc.Load(ms, Encoding.UTF8);
                HtmlNode tempNode;

                tempNode = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/table[1]/tr[1]/td[2]/center[3]/table[1]/center[1]/table[1]");

                // 取得匯率 
                for (int i = 2; i < 12; i++)
                {
                    CRBList[i-2].Oil = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + i.ToString() + "]/td[2]").InnerText);
                    CRBList[i+8].Oil = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + i.ToString() + "]/td[5]").InnerText);
                }

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

        public List<CRB_BDI> getCRB_BDI()
        {
            try
            {
                IQueryable<CRB_BDI> SearchData = db.CRB_BDI;
                return SearchData.ToList().OrderByDescending(p => p.Publish_Date).ToList();
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public void UpdateBullAndBear()
        {
            try
            {
                BullAndBear tempItem = new BullAndBear();

                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("http://www.wantgoo.com/stock/adl"));
                //以台灣銀行為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                doc.Load(ms, Encoding.UTF8);
                HtmlNode tempNode;

                tempNode = doc.DocumentNode.CssSelect("#mainCol").ToList()[0];

                tempItem.Publish_Date = DateTime.Parse(tempNode.SelectSingleNode(@"div[1]/span[2]").InnerText);
                tempItem.Bull = 0;//Decimal.Parse(tempNode.SelectSingleNode(@"div[2]/table[1]/tbody[1]/tr[1]/td[5]").InnerText);
                tempItem.Limit_Up = Decimal.Parse(tempNode.SelectSingleNode(@"div[2]/table[1]/tbody[1]/tr[1]/td[6]").InnerText);
                tempItem.Red_Candle = Decimal.Parse(tempNode.SelectSingleNode(@"div[2]/table[1]/tbody[1]/tr[1]/td[7]").InnerText);
                tempItem.Bear = 0;//Decimal.Parse(tempNode.SelectSingleNode(@"div[2]/table[1]/tbody[1]/tr[1]/td[8]").InnerText);
                tempItem.Limit_Down = Decimal.Parse(tempNode.SelectSingleNode(@"div[2]/table[1]/tbody[1]/tr[1]/td[9]").InnerText);
                tempItem.Black_Candle = Decimal.Parse(tempNode.SelectSingleNode(@"div[2]/table[1]/tbody[1]/tr[1]/td[10]").InnerText);

                db.BullAndBears.Add(tempItem);
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

        public void UpdateCYQ(int Num)
        {
            try
            {
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("https://histock.tw/stock/large.aspx?no=" + Num));
                //以GoogleFinance為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                HtmlNode tempNode;
                doc.Load(ms, Encoding.UTF8);

                tempNode = doc.DocumentNode.CssSelect("table.tb-stock").ToList()[1];

                // 取得股價 
                for (int num = 3; num < 28; num++)
                {
                    CYQ CYQ_temp = new CYQ();

                    CYQ_temp.Stock_Number = Num;
                    CYQ_temp.Publish_Week = DateTime.Parse(tempNode.SelectSingleNode(@"tr[" + num.ToString() + "]/td[1]").InnerText);
                    CYQ_temp.CYQ1 = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + num.ToString() + "]/td[2]").InnerText.Replace("%", ""));
                    CYQ_temp.Foreign_Investment = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + num.ToString() + "]/td[3]").InnerText.Replace("%", ""));
                    CYQ_temp.Major_Player = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + num.ToString() + "]/td[4]").InnerText.Replace("%", ""));
                    CYQ_temp.Supervisor = Decimal.Parse(tempNode.SelectSingleNode(@"tr[" + num.ToString() + "]/td[5]").InnerText.Replace("%", ""));

                    db.CYQs.Add(CYQ_temp);
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

        public List<CYQ> getCYQ(string Num)
        {
            try
            {
                IQueryable<CYQ> SearchData;
                bool numIsEmpty = String.IsNullOrEmpty(Num);

                if (!numIsEmpty)
                {
                    int Id = int.Parse(Num);
                    SearchData = db.CYQs.Where(p => p.Stock_Number.Equals(Id));

                    if (SearchData.Count() == 0)
                    {
                        List<CYQ> nullCYQ = new List<CYQ>();
                        return nullCYQ;
                    }
                    else
                    {
                        return SearchData.ToList().OrderByDescending(p => p.Publish_Week).ToList();
                    }
                }
                else
                {
                    List<CYQ> nullCYQ = new List<CYQ>();
                    return nullCYQ;
                }
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public List<MarketItem> getMarketList()
        {
            try
            {
                List<MarketItem> StocksList = new List<MarketItem>();
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("http://www.wantgoo.com/"));
                //以GoogleFinance為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                HtmlNode tempNode;
                doc.Load(ms, Encoding.UTF8);

                //tempNode = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[2]/div[2]/div[2]/div[7]/div[2]/div[2]");
                tempNode = doc.DocumentNode.CssSelect("#tabRK1").ToList()[0];
                // 取得股價 
                for (int i = 1; i < 5; i++)
                {
                    for (int j = 1; j < 6; j++)
                    {
                        MarketItem tempItem = new MarketItem();

                        tempItem.Order = tempNode.SelectSingleNode(@"table[" + i.ToString() + "]/tbody[1]/tr[" + j.ToString() + "]/td[1]").InnerText;
                        tempItem.Name = tempNode.SelectSingleNode(@"table[" + i.ToString() + "]/tbody[1]/tr[" + j.ToString() + "]/td[2]").InnerText;
                        tempItem.Amplitude = tempNode.SelectSingleNode(@"table[" + i.ToString() + "]/tbody[1]/tr[" + j.ToString() + "]/td[3]").InnerText;

                        StocksList.Add(tempItem);
                    }
                }

                //清除資料
                doc = null;
                url = null;
                ms.Close();

                return StocksList;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public void UpdateVouchers(int Num)
        {
            try
            {
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("http://www.wantgoo.com/stock/astock/margin?StockNo=" + Num));
                //以GoogleFinance為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                HtmlNode tempNode1, tempNode2;
                doc.Load(ms, Encoding.UTF8);

                tempNode1 = doc.DocumentNode.CssSelect("#tab1_list").ToList()[0];
                tempNode2 = doc.DocumentNode.CssSelect("#tab2_list").ToList()[0];

                // 取得股價 
                for (int num = 1; num < 31; num++)
                {
                    Voucher Voucher_temp = new Voucher();

                    Voucher_temp.Stock_Number = Num;
                    Voucher_temp.Publish_Date = DateTime.Parse(tempNode1.SelectSingleNode(@"table[1]/tbody[1]/tr[" + num.ToString() + "]/td[1]").InnerText);
                    Voucher_temp.VoucherRatio = decimal.Parse(tempNode1.SelectSingleNode(@"table[1]/tbody[1]/tr[" + num.ToString() + "]/td[6]").InnerText);
                    Voucher_temp.DayTradingRatio = decimal.Parse(tempNode1.SelectSingleNode(@"table[1]/tbody[1]/tr[" + num.ToString() + "]/td[8]").InnerText);
                    Voucher_temp.FinancingValue = decimal.Parse(tempNode2.SelectSingleNode(@"table[1]/tbody[1]/tr[" + num.ToString() + "]/td[4]").InnerText);
                    Voucher_temp.MarginValue = decimal.Parse(tempNode2.SelectSingleNode(@"table[1]/tbody[1]/tr[" + num.ToString() + "]/td[7]").InnerText);

                    db.Vouchers.Add(Voucher_temp);
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

        public List<Voucher> getVoucher(string Num)
        {
            try
            {
                IQueryable<Voucher> SearchData;
                bool numIsEmpty = String.IsNullOrEmpty(Num);

                if (!numIsEmpty)
                {
                    int Id = int.Parse(Num);
                    SearchData = db.Vouchers.Where(p => p.Stock_Number.Equals(Id));

                    if (SearchData.Count() == 0)
                    {
                        List<Voucher> nullVoucher = new List<Voucher>();
                        return nullVoucher;
                    }
                    else
                    {
                        return SearchData.ToList().OrderByDescending(p => p.Publish_Date).ToList();
                    }
                }
                else
                {
                    List<Voucher> nullVoucher = new List<Voucher>();
                    return nullVoucher;
                }
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public string[] getVIXList()
        {
            try
            {
                string[] VIXList = new string[10];
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("https://histock.tw/index/VIX"));
                //以GoogleFinance為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                HtmlNode tempNode;
                doc.Load(ms, Encoding.UTF8);

                tempNode = doc.DocumentNode.CssSelect("div.tb-outline").ToList()[2];
                // 取得股價 
                for (int i = 3; i < 13; i++)
                {
                    VIXList[i - 3] = tempNode.SelectSingleNode(@"table[1]/tbody[1]/tr[" + i.ToString() + "]/td[1]").InnerText + "," + tempNode.SelectSingleNode(@"table[1]/tbody[1]/tr[" + i.ToString() + "]/td[2]").InnerText;
                }

                //清除資料
                doc = null;
                url = null;
                ms.Close();

                return VIXList;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public string[] getLegalPerson()
        {
            try
            {
                string[] LegalPerson = new string[10];
                //指定來源網頁
                WebClient url = new WebClient();
                //將網頁來源資料暫存到記憶體內
                MemoryStream ms = new MemoryStream(url.DownloadData("http://www.wantgoo.com/"));
                //以GoogleFinance為範例

                // 使用預設編碼讀入 HTML 
                HtmlDocument doc = new HtmlDocument();
                HtmlNode tempNode;
                doc.Load(ms, Encoding.UTF8);

                tempNode = doc.DocumentNode.CssSelect("#tabMix1").ToList()[0];
                // 取得股價 
                for (int j = 1; j < 3; j++)
                {
                    LegalPerson[(j - 1) * 5] = tempNode.SelectSingleNode(@"div[1]/table[" + j.ToString() + "]/caption[1]").InnerText;
                    for (int i = 1; i < 5; i++)
                    {
                        LegalPerson[i + (j - 1) * 5] = tempNode.SelectSingleNode(@"div[1]/table[" + j.ToString() + "]/tbody[1]/tr[1]/td[" + i.ToString() + "]").InnerText;
                    }
                }

                //清除資料
                doc = null;
                url = null;
                ms.Close();

                return LegalPerson;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public decimal[] getForceRatio()
        {
            try
            {
                decimal[] ForceRatio = new decimal[3];
                List<BullAndBear> Force = new List<BullAndBear>();
                Force = db.BullAndBears.OrderByDescending(p => p.Publish_Date).Take(ForceRatio.Count()).ToList();
                for (int i = 0; i < ForceRatio.Count(); i++)
                {
                    ForceRatio[i] = Force[i].Red_Candle / Force[i].Black_Candle;
                }

                return ForceRatio;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }
    }
}