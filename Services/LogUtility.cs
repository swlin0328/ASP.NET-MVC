using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Services
{
    public static class LogUtility
    {
        public static string GetExceptionDetails (Exception ex)
        {
            Exception logException = ex;

            if (ex.InnerException != null)
            {
                logException = ex.InnerException;
            }

            System.Text.StringBuilder message = new System.Text.StringBuilder();
            message.AppendLine();
            message.AppendLine("要求虛擬路徑: " + HttpContext.Current.Request.Path);
            message.AppendLine("要求原始URL: " + HttpContext.Current.Request.RawUrl);
            message.AppendLine("例外類型: " + logException.GetType().Name);
            message.AppendLine("例外訊息: " + logException.Message);
            message.AppendLine("例外來源: " + logException.Source);
            message.AppendLine("Stack Trace: " + logException.StackTrace);
            message.AppendLine("TargetSite: " + logException.TargetSite);

            return message.ToString();
        }
    }
}