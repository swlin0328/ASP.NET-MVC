using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;
using WebApplication3.Services;
using System.ComponentModel;

namespace WebApplication3.ViewModels
{
    public class ExchangeView
    {
        [DisplayName("幣別代碼：")]
        public string Search { get; set; }
        public List<Exchange> ExchangeList { get; set; }
        public ForPaging Paging { get; set; }
    }
}