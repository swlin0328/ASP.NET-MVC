using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    [MetadataType(typeof(StockMetadata))]
    public partial class Stock
    {
        private class StockMetadata
        {
            [DisplayName("股票代號")]
            public int Id { get; set; }
            [DisplayName("名稱")]
            public string stockName { get; set; }
            [DisplayName("成交量")]
            public int Volume { get; set; }
            [DisplayName("資料日期")]
            public System.DateTime Price_Date { get; set; }
            [DisplayName("開盤")]
            public decimal Price_Open { get; set; }
            [DisplayName("最高")]
            public decimal Price_High { get; set; }
            [DisplayName("最低")]
            public decimal Price_Low { get; set; }
            [DisplayName("收盤")]
            public decimal Price_Close { get; set; }
        }
    }
}