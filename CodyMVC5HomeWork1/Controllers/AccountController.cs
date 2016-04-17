using CodyMVC5HomeWork1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CodyMVC5HomeWork1.Controllers
{
    public class AccountController : Base2Controller
    {
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [HandleError(View = "Error2")]
        public ActionResult Login(LoginViewModel data,string returnUrl)
        {
            FormsAuthentication.SignOut();
            Session.RemoveAll();

            if (CheckLogin(data))
            {
                bool isPersistent = false;
                string userData = "";

                string userid=repo客戶資料.GetUserId(data.帳號);
                if (!string.IsNullOrEmpty(userid))
                    userData = userid ;

                if (data.帳號 == "admin")
                    userData += ",RoleAdmin,NormalUser";
                else
                    userData += ",NormalUser";


                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                  repo客戶資料.GetUserName(data.帳號),
                  DateTime.Now,
                  DateTime.Now.AddMinutes(30),
                  isPersistent,
                  userData,
                  FormsAuthentication.FormsCookiePath);

                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Index", "Home");
            }

            TempData["LoginFail"] = "帳號密碼錯誤！！";
            FormsAuthentication.SignOut();
            return View(data);
        }

        [AllowAnonymous]
        private bool CheckLogin(LoginViewModel data)
        {
            

            var sourcepw = repo客戶資料.GetHashPW(data.帳號);
            var inputpw = FormsAuthentication.HashPasswordForStoringInConfigFile(data.密碼, "SHA1");
            if (sourcepw == inputpw)
                return true;
            else
                return false;

        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");

        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "RoleAdmin,NormalUser")]
        [HandleError(View = "Error2")]
        public ActionResult EditProfile()
        {

            string userid = GetLoginId();
            if (!string.IsNullOrEmpty(userid))
            {
                var data = repo客戶資料.Find(int.Parse(userid));
                return View(data);
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Account");
            }
        }

        [Authorize(Roles = "RoleAdmin,NormalUser")]
        [HandleError(View = "Error2")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(FormCollection collection)
        {
            //if (!string.IsNullOrEmpty(客戶資料.密碼))
            //    客戶資料.密碼 = FormsAuthentication.HashPasswordForStoringInConfigFile(客戶資料.密碼, "SHA1");

            string userid = GetLoginId();
            if (!string.IsNullOrEmpty(userid))
            {
                var data = repo客戶資料.Find(int.Parse(userid));
                data.電話 = collection["電話"];
                data.傳真 = collection["傳真"];
                data.地址 = collection["地址"];

                data.Email = collection["Email"] == "" ? null : collection["Email"];
                data.密碼 = FormsAuthentication.HashPasswordForStoringInConfigFile(collection["密碼"], "SHA1");
                repo客戶資料.UnitOfWork.Commit();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["UpdateFail"] = "更新失敗，請重新登入後再試！！";
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Account");
            }

            
        }

        public string GetLoginId()
        {
            FormsIdentity id = (FormsIdentity)User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            string[] roles = ticket.UserData.Split(new char[] { ',' });
            string userid = roles[0];
            return userid;
        }

    }
}