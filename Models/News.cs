using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace WebApplication3.Models
{
    public class News
    {
        [DisplayName("標題")]
        public string title { get; set; }
        [DisplayName("連結")]
        public string link { get; set; }
        [DisplayName("發佈日期")]
        public string publishDate { get; set; }
        [DisplayName("摘要")]
        public string description { get; set; }
    }
}