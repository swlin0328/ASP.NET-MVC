using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace WebApplication3.Services
{
    public class MailService
    {
        private string gmail_account = "swlin0328";
        private string gmail_password = "xnpsxyfglpshfmsv";
        private string gmail_mail = "swlin0328@gmail.com";

        public string GetValidateCode()
        {
            string[] Code = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "0", "1", "2", "3", "4", "5", "z", "x", "c", "v", "b", "n" };
            string ValidateCode = string.Empty;
            Random rd = new Random();
            for (int i = 0; i < 10; i++)
            {
                ValidateCode += Code[rd.Next(Code.Count())];
            }
            return ValidateCode;
        }

        public string GetRegisterMailBody(string TempString, string UserName, string ValidateUrl)
        {
            TempString = TempString.Replace("{{UserName}}", UserName);
            TempString = TempString.Replace("{{ValidateUrl}}", ValidateUrl);
            return TempString;
        }

        public void SendRegisterMail(string MailBody, string ToEmail)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(gmail_account,gmail_password);
            SmtpServer.EnableSsl = true;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(gmail_mail);
            mail.To.Add(ToEmail);
            mail.Subject = "會員註冊確認信";
            mail.Body = MailBody;
            mail.IsBodyHtml = true;
            SmtpServer.Send(mail);
        }
    }
}