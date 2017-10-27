using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;
using WebApplication3.Services;

namespace WebApplication3.ViewModels
{
    public class NewsView
    {
        public List<News> NewsList { get; set; }
        public List<News> GlobalNewsList { get; set; }
        public ForPaging Paging { get; set; }
    }
}