using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;
using System.ComponentModel;
using WebApplication3.Services;

namespace WebApplication3.ViewModels
{
    public class GuestbookView
    {
        [DisplayName("搜尋：")]
        public string Search { get; set; }
        public List<Guestbooks> DataList { get; set; }
        public ForPaging Paging { get; set; }
    }
}