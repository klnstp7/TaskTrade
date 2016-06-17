using System.Collections.Specialized;
using DotNetCasClient;
using Peacock.Common.Helper;
using System.Configuration;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Web; 

namespace Peacock.PEP.MvcWebSite.Controllers
{
    
    public class HomeController : BaseController
    {
        
        
        public ActionResult Index()
        {
            var UserAccount = CookieHelper.GetUserAccount();
            string url = ConfigurationManager.AppSettings["ReLogInUrl"];
            if (Request.QueryString["service"] != null)
            {
                if (Request.QueryString["service"] == url)
                {
                    CookieHelper.RemoveCookie(CookieHelper.UserStateKey);
                    CasAuthentication.SingleSignOut();
                    LogHelper.Ilog("Index?UserAccount=" + UserAccount, UserAccount + "注销");
                }
            }
            else
            {
                LogHelper.Ilog("Index?UserAccount=" + UserAccount, UserAccount + "登录");
            }           
            ViewBag.UserAccount = UserAccount;
            ViewBag.IsSupperUser = ConfigurationManager.AppSettings["AdministratorList"].Split(',').Contains(UserAccount);
            ViewBag.PowerSysUrl = ConfigurationManager.AppSettings["PowerSysLoginUrl"] + "?UserName=" + UserAccount;
            ViewBag.IsCompanyManage = UserService.GetUser().UserType==2;
            ViewBag.ReLogInUrl = url;
            ViewBag.WaiCaiUrl = ConfigurationManager.AppSettings["WaiCaiUrl"];
            var citys = ConfigurationManager.GetSection("xunjiaCitys") as NameValueCollection;
            ViewBag.Citys = citys;
            ViewBag.City = CurrentCity;
            return View();

        }

        public ActionResult NoPermission()
        {
            return View();
        }
        public ActionResult Welcome()
        {
            return View();
        }


        public void ChangeCity(string cityName)
        {
            CurrentCity = cityName;
        }

    }
}
