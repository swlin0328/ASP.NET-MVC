using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Security.Principal;
using System.Web.Security;
using System.Threading;

namespace WebApplication3
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            IPrincipal contextUser = Context.User;

            if (contextUser.Identity.AuthenticationType == "Forms")
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;
                string[] roles = ticket.UserData.Split(new char[] { ',' });
                HttpContext.Current.User = new GenericPrincipal(User.Identity, roles);
                Thread.CurrentPrincipal = HttpContext.Current.User;
            }
        }

      /*  void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            bool hasUser = HttpContext.Current.User != null;
            bool isAuthenticated = hasUser && HttpContext.Current.User.Identity.IsAuthenticated;
            bool isIdentity = isAuthenticated && HttpContext.Current.User.Identity is FormsIdentity;
            if (isIdentity)
            {
                // 先取得該使用者的 FormsIdentity
                FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                // 再取出使用者的 FormsAuthenticationTicket
                FormsAuthenticationTicket ticket = id.Ticket;
                // 將儲存在 FormsAuthenticationTicket 中的角色定義取出，並轉成字串陣列
                string[] roles = ticket.UserData.Split(new char[] { ',' });
                // 指派角色到目前這個 HttpContext 的 User 物件去
                HttpContext.Current.User = new GenericPrincipal(id, roles);
            }
        }*/
    }
}
