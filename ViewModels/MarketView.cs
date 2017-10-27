using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using WebApplication3.Models;

namespace WebApplication3.ViewModels
{
    public class MarketView
    {
        public List<MarketItem> MarketList { get; set; }
        [DisplayName("股票代號：")]
        public string Search { get; set; }
        public string[] VIXList { get; set; }
        public string[] LegalPerson { get; set; }
        public decimal[] ForceRatio { get; set; }
    }
}