using Lawave.Models;
using Lawave.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Lawave.Areas.admin.Controllers
{
    public class BackAccountController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: BackAccount
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = db.Accounts.Where(x => x.AccountName == model.UserName && x.Password == model.Password).FirstOrDefault();

            if (user == null)
            {
                ModelState.AddModelError("", "無效的帳號或密碼。");
                return View();
            }

            var ticket = new FormsAuthenticationTicket(
                        version: 1,
                        name: user.AccountName.ToString(), //可以放使用者Id
                        issueDate: DateTime.Now,//現在UTC時間
                        expiration: DateTime.Now.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
                        isPersistent: true,// 是否要記住我 true or false
                        userData: user.AccountName, //可以放使用者角色名稱
                        cookiePath: FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket); //把驗證的表單加密
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Home");
        }
    }
}