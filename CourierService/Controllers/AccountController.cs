using CourierService.Services.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using static System.Net.WebRequestMethods;
using Http = System.Web.HttpContext;

namespace CourierService.Controllers
{
    public class AccountController : Controller
    {

        AccountService _service = new AccountService();
        // GET: Account
        public ActionResult Login()
        {


            return View();
        }
        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            var user = _service.Login(userName, password);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Username, false);

                Session["Role"] = user.RoleId;
                Session["UserModel"] = user.Username;
                Session["UserId"] = user.Id;

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);

            }

            
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = Utility.getDefaultTime().AddYears(-1);//DateTime.Now.AddYears(-1)
            Response.Cookies.Add(cookie1);
            SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            HttpCookie cookie2 = new HttpCookie(sessionStateSection.CookieName, "");
            cookie2.Expires = Utility.getDefaultTime().AddYears(-1);//DateTime.Now.AddYears(-1)
            Response.Cookies.Add(cookie2);

            Session["UserToken"] = null;
            Session["Role"] = null;
            Session["UserModel"] = null;
            Session["UserId"] = null;
            return RedirectToAction("Login");
        }
    }
}