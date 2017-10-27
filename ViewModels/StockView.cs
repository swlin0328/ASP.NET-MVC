using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;
using WebApplication3.Services;
using System.ComponentModel;

namespace WebApplication3.ViewModels
{
    public class StockView
    {
        [DisplayName("股票代號：")]
        public string Search { get; set; }
        public List<Stock> StockList { get; set; }
        public ForPaging Paging { get; set; }
    }
}