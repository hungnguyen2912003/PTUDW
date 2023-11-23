using MyClass.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace THPTUDWeb.Controllers
{
    public class ModuleController : Controller
    {
        // GET: Module
        MenusDAO menusDAO = new MenusDAO();
        ///////////////////////////////////////////////////////////////////
        //GET: MainMenu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MainMenu()
        {
            return View(menusDAO.getListByParentId(0));
        }
    }
}