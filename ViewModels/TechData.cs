using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;

namespace WebApplication3.ViewModels
{
    public class TechData
    {
        public int Search { get; set; }
        public List<Stock> StockList { get; set; }
        public List<CYQ> CYQList { get; set; }
        public List<MonetaryAggregate> MonetaryList { get; set; }
        public List<Libor> LiborList { get; set; }
        public List<Voucher> VoucherList { get; set; }
        public List<CRB_BDI> CRB_BDIList { get; set; }
    }
}