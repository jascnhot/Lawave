using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lawave.Areas.admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {        
        public ActionResult Index()
        {
           
            ViewData["Login_Account"] = User.Identity.Name;
            
            return View();
        }
        public ActionResult Chat()
        {
            return View();
        }

      
    }
}
