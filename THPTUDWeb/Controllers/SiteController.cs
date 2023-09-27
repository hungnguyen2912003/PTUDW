using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace THPTUDWeb.Controllers
{
    public class SiteController : Controller
    {
        // GET: Site
        public ActionResult Index()
        {
            MyDBContext db = new MyDBContext(); //Tao Database
            int sodong = db.Products.Count();
            ViewBag.sodong = sodong;
            return View();
        }
    }
}