using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Services;
using WebApplication3.ViewModels;
using System.Web.Security;

namespace WebApplication3.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member
        private MemberService memberService = new MemberService();
        private MailService mailService = new MailService();

        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            return RedirectToAction("Index","Home");
            return View();
        }

        [HttpPost]
        public ActionResult Register(MemberRegisterView RegisterMember)
        {
            if (ModelState.IsValid)
            {
                string AuthCode = mailService.GetValidateCode();
                string TempMail = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/RegisterEmailTemplate.html"));
                
                RegisterMember.newMember.Password = RegisterMember.Password;
                RegisterMember.newMember.AuthCode = AuthCode;
                memberService.Register(RegisterMember.newMember);

                UriBuilder ValidateUrl = new UriBuilder(Request.Url)
                {
                    Path = Url.Action("EmailValidate", "Member", new { UserName = RegisterMember.newMember.Account, AuthCode = AuthCode })
                };

                string MailBody = mailService.GetRegisterMailBody(TempMail, RegisterMember.newMember.Name, ValidateUrl.ToString().Replace("%3F", "?"));
                mailService.SendRegisterMail(MailBody, RegisterMember.newMember.Email);
                TempData["RegisterState"] = "註冊成功，請去收信以驗證Email";

                return RedirectToAction("RegisterResult");
            }
            RegisterMember.Password = null;
            RegisterMember.PasswordCheck = null;
            return View(RegisterMember);
        }

        public ActionResult RegisterResult()
        {
            return View();
        }

        public JsonResult AccountCheck(MemberRegisterView RegisterMember)
        {
            return Json(memberService.AccountCheck(RegisterMember.newMember.Account),JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmailValidate(string UserName, string AuthCode)
        {
            ViewData["EmailValidate"] = memberService.EmailValidate(UserName,AuthCode);
            return View();
        }

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(MemberLoginView LoginMember)
        {
            string ValidateStr = memberService.LoginCheck(LoginMember.UserName, LoginMember.Password);
            
            if (String.IsNullOrEmpty(ValidateStr))
            {
                string RoleData = memberService.GetRole(LoginMember.UserName.ToLower());
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, LoginMember.UserName.ToLower(), DateTime.Now, DateTime.Now.AddMinutes(30), true, RoleData, FormsAuthentication.FormsCookiePath);
                string enTicket = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, enTicket);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Guestbook");
            }
            else
            {
                ModelState.AddModelError("",ValidateStr);
                return View(LoginMember);
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordView ChangeData)
        {
            if (ModelState.IsValid)
            {
                ViewData["ChangeState"] = memberService.ChangePassword(User.Identity.Name, ChangeData.Password, ChangeData.NewPassword);
            }
            return View();
        }
    }
}