using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication3.Services
{
    public class MemberService
    {
        private GuestbooksEntities db = new GuestbooksEntities();
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void Register(Members newMember)
        {
            try
            {
                newMember.Password = HashPassword(newMember.Password);
                newMember.Account = newMember.Account.ToLower();
                newMember.IsAdmin = false;
                db.Members.Add(newMember);
                db.SaveChanges();
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return;
            }
        }

        public String HashPassword(string Password)
        {
            try
            {
                string saltkey = "asdfg12345zxcvb";
                string saltAndPassword = String.Concat(Password, saltkey);
                SHA1CryptoServiceProvider sha1Hasher = new SHA1CryptoServiceProvider();
                byte[] PasswordData = Encoding.Default.GetBytes(saltAndPassword);
                byte[] HashDate = sha1Hasher.ComputeHash(PasswordData);
                string Hashresult = "";

                for (int i = 0; i < HashDate.Length; i++)
                {
                    Hashresult += HashDate[i].ToString("x2");
                }
                return Hashresult;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public bool AccountCheck(string Account)
        {
            try
            {
                Members Search = db.Members.Find(Account.ToLower());
                bool result = (Search == null);
                return result;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return false;
            }
        }

        public string EmailValidate(string UserName, string AuthCode)
        {
            try
            {
                Members ValidateMember = db.Members.Find(UserName);
                string ValidateStr = string.Empty;
                if (ValidateMember != null)
                {
                    if (ValidateMember.AuthCode == AuthCode)
                    {
                        ValidateMember.AuthCode = string.Empty;
                        db.SaveChanges();
                        ValidateStr = "帳號信箱驗證成功，現在可以登入了";
                    }
                    else
                    {
                        ValidateStr = "驗證碼錯誤，請重新確認在註冊";
                    }
                }
                else
                {
                    ValidateStr = "傳送資料錯誤，請重新確認或再註冊";
                }
                return ValidateStr;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public string LoginCheck(string UserName, string Password)
        {
            try
            {
                Members LoginMember = db.Members.Find(UserName.ToLower());
                if (LoginMember != null)
                {
                    if (String.IsNullOrWhiteSpace(LoginMember.AuthCode))
                    {
                        if (PasswordCheck(LoginMember, Password))
                        {
                            return "";
                        }
                        else
                        {
                            return "密碼輸入錯誤";
                        }
                    }
                    else
                    {
                        return "此帳號尚未經過Email驗證，請去收信";
                    }
                }
                else
                {
                    return "無此會員帳號，請去註冊";
                }
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public bool PasswordCheck(Members CheckMember, string Password)
        {
            try
            {
                bool result = CheckMember.Password.Equals(HashPassword(Password));
                return result;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return false;
            }
        }

        public string GetRole(string UserName)
        {
            try
            {
                string Role = "User";
                Members LoginMember = db.Members.Find(UserName);

                if (LoginMember.IsAdmin)
                {
                    Role += ",Admin";
                }
                return Role;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public string ChangePassword(string UserName, string Password, string newPassword)
        {
            try
            {
                Members LoginMember = db.Members.Find(UserName.ToLower());
                if (PasswordCheck(LoginMember, Password))
                {
                    LoginMember.Password = HashPassword(newPassword);
                    db.SaveChanges();
                    return "密碼修改成功";
                }
                else
                {
                    return "舊密碼輸入錯誤";
                }
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }
    }
}