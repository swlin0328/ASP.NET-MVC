using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{

    [MetadataType(typeof(ExchangeMetadata))]
    public partial class Exchange
    {
        private class ExchangeMetadata
        {
            [DisplayName("幣別")]
            public string Currency { get; set; }

            [DisplayName("掛牌日期")]
            public DateTime QuotedDate { get; set; }

            [DisplayName("現金買進")]
            public decimal CashRate_Buying { get; set; }

            [DisplayName("現金賣出")]
            public decimal CashRate_Selling { get; set; }

            [DisplayName("即期買進")]
            public decimal SpotRate_Buying { get; set; }

            [DisplayName("即期賣出")]
            public decimal SpotRate_Selling { get; set; }
        }
    }
}