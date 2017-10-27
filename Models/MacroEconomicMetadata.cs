using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    [MetadataType(typeof(MacroeconomicMetadata))]
    public partial class Macroeconomic
    {
        private class MacroeconomicMetadata
        {
            [DisplayName("日期")]
            public System.DateTime Publish_Date { get; set; }
            [DisplayName("消費者物價指數")]
            public decimal CPI { get; set; }
            [DisplayName("年增%")]
            public decimal CPI_GrowthRate { get; set; }
            [DisplayName("消費者核心物價指數")]
            public decimal CCPI { get; set; }
            [DisplayName("年增%")]
            public decimal CCPI_GrowthRate { get; set; }
            [DisplayName("生產者物價指數核心年增%")]
            public decimal PPI_GrowthRate { get; set; }
            [DisplayName("失業率%")]
            public decimal Unemployment_Rate { get; set; }
            [DisplayName("非農業就業人口增減")]
            public int NFP { get; set; }
            [DisplayName("平均每週工作時數")]
            public decimal Weekly_Work_Hours { get; set; }
            [DisplayName("平均每小時工資年增%")]
            public decimal Hourly_WageRate { get; set; }
            [DisplayName("出口年增%")]
            public decimal Export_IncreasedRate { get; set; }
            [DisplayName("出口價格指數年增%")]
            public decimal Export_PriceRate { get; set; }
            [DisplayName("進口年增%")]
            public decimal Import_IncreasedRate { get; set; }
            [DisplayName("進口價格指數年增%")]
            public decimal Import_PriceRate { get; set; }
            [DisplayName("新屋開工年化(千)")]
            public decimal Housing_Starts { get; set; }
            [DisplayName("年增%")]
            public decimal Housing_Starts_GrowthRate { get; set; }
            [DisplayName("零售銷售年增%")]
            public decimal Retail_Sales_Rate { get; set; }
            [DisplayName("零售銷售不含汽車年增%")]
            public decimal Retail_Sales_Rate_NoCar { get; set; }
            [DisplayName("個人支出年增%")]
            public decimal Personal_Expenditure_Rate { get; set; }
            [DisplayName("個人所得年增%")]
            public decimal Individual_Income_Rate { get; set; }
            [DisplayName("密西根消費者信心指數")]
            public decimal ICS { get; set; }
            [DisplayName("產能利用率%")]
            public decimal Capacity_Utilization { get; set; }
            [DisplayName("耐久財訂單年增%")]
            public decimal Durable_Orders { get; set; }
            [DisplayName("工廠訂單年增%")]
            public decimal Factory_Orders { get; set; }
            [DisplayName("工廠庫存年增%")]
            public decimal Factory_Stock { get; set; }
            [DisplayName("工廠出貨年增%")]
            public decimal Factory_Shipments { get; set; }
            [DisplayName("ISM製造業採購經理指數")]
            public decimal Manufacturing_PMI { get; set; }
            [DisplayName("芝加哥採購經理人指數")]
            public decimal PMI { get; set; }
        }
    }
}