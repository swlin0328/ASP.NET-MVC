using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;
using System.IO;
using System.Text;
using System.Net;
using WebApplication3.ViewModels;
using System.Xml;
using System.Data;
using System.Collections;

namespace WebApplication3.Services
{
    public class RSS
    {
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public NewsView RSSNews()
        {
            try
            {
                string[] StockURL = new string[]{
                "https://tw.finance.yahoo.com/rss/url/d/e/R1.html",
                "https://tw.finance.yahoo.com/rss/url/d/e/N2.html",
                "https://tw.finance.yahoo.com/rss/url/d/e/N10.html",
                "https://tw.finance.yahoo.com/rss/url/d/e/N11.html",
                "https://tw.finance.yahoo.com/rss/url/d/e/R3.html"
            };

                string[] GlobalURL = new string[]{
                "https://tw.finance.yahoo.com/rss/url/d/e/N12.html",
                "https://tw.finance.yahoo.com/rss/url/d/e/R6.html"
            };

                NewsView StockNews = new NewsView();
                XmlDocument xmlDoc = new XmlDocument();
                XmlNodeList publishList, titleList, descriptionList, linkList;
                StockNews.NewsList = new List<News>();
                StockNews.GlobalNewsList = new List<News>();

                for (int j = 0; j < StockURL.Count(); j++)
                {
                    xmlDoc.Load(StockURL[j]);
                    publishList = xmlDoc.SelectNodes("//pubDate");
                    titleList = xmlDoc.SelectNodes("//title");
                    descriptionList = xmlDoc.SelectNodes("//description");
                    linkList = xmlDoc.SelectNodes("//link");

                    for (int i = 2; i < 7; i++)
                    {
                        News TempNews = new News();

                        TempNews.publishDate = publishList[i].InnerText;
                        TempNews.title = titleList[i].InnerText;
                        TempNews.description = descriptionList[i].InnerText;
                        TempNews.link = linkList[i].InnerText;

                        StockNews.NewsList.Add(TempNews);
                    }
                }

                for (int j = 0; j < GlobalURL.Count(); j++)
                {
                    xmlDoc.Load(StockURL[j]);
                    publishList = xmlDoc.SelectNodes("//pubDate");
                    titleList = xmlDoc.SelectNodes("//title");
                    descriptionList = xmlDoc.SelectNodes("//description");
                    linkList = xmlDoc.SelectNodes("//link");

                    for (int i = 2; i < 10; i++)
                    {
                        News TempNews = new News();

                        TempNews.publishDate = publishList[i].InnerText;
                        TempNews.title = titleList[i].InnerText;
                        TempNews.description = descriptionList[i].InnerText;
                        TempNews.link = linkList[i].InnerText;

                        StockNews.GlobalNewsList.Add(TempNews);
                    }
                }

                StockNews.NewsList = StockNews.NewsList.OrderByDescending(p => p.publishDate).Distinct(new NewsDataRowComparer()).Take(5).ToList();
                StockNews.GlobalNewsList = StockNews.GlobalNewsList.OrderByDescending(p => p.publishDate).Distinct(new NewsDataRowComparer()).Take(5).ToList();

                return StockNews;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public class NewsDataRowComparer : IEqualityComparer<News>
        {
            public bool Equals(News t1, News t2)
            {
                return (t1.publishDate == t2.publishDate);
            }

            public int GetHashCode(News t)
            {
                return t.ToString().GetHashCode();
            }
        }
    }

}