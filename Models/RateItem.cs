using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace WebApplication3.Models
{
    public class RateItem
    {
        public string Country { get; set; }
        [DisplayName("利率名稱")]
        public string RateName { get; set; }
        [DisplayName("目前利率")]
        public string Rate { get; set; }
        [DisplayName("升降基點")]
        public string point { get; set; }
        [DisplayName("公布日期")]
        public string pubDate { get; set; }
        [DisplayName("前次利率")]
        public string LastRate { get; set; }
        [DisplayName("通膨率")]
        public string Inflation { get; set; }
    }
}