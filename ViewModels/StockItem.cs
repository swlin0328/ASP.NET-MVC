using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
    public class StockItem
    {
        [DisplayName("股票代號：")]
        public string Search { get; set; }
        [DisplayName("名稱代號")]
        public string Name { get; set; }
        [DisplayName("時間")]
        public string Time { get; set; }
        [DisplayName("成交")]
        public decimal Deal { get; set; }
        [DisplayName("買進")]
        public decimal Buy { get; set; }
        [DisplayName("賣出")]
        public decimal Sell { get; set; }
        [DisplayName("漲跌")]
        public string Change { get; set; }
        [DisplayName("成交量")]
        public int Volume { get; set; }
        [DisplayName("昨收")]
        public decimal lastDay { get; set; }
        [DisplayName("開盤")]
        public decimal Open { get; set; }
        [DisplayName("最高")]
        public decimal High { get; set; }
        [DisplayName("最低")]
        public decimal Low { get; set; }
    }
}