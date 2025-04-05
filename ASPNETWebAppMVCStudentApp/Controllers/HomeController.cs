//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//// Test pushing
//namespace ASPNETWebAppMVCStudentApp.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ActionResult Index()
//        {
//            return View();
//        }

//        public ActionResult About()
//        {
//            ViewBag.Message = "Your application description page.";

//            return View();
//        }

//        public ActionResult Contact()
//        {
//            ViewBag.Message = "Your contact page.";

//            return View();
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPNETWebAppMVCStudentApp.Models;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class HomeController : Controller
    {
        private SchoolDBEntities db = new SchoolDBEntities();

        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.tblUsers.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    Session["Username"] = user.Username;
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
