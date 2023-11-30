using MyClass.DAO;
using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace THPTUDWeb.Controllers
{
    public class ModuleController : Controller
    {
        // GET: Module
        MenusDAO menusDAO = new MenusDAO();
        ///////////////////////////////////////////////////////////////////////////
        // GET: Mainmenu
        public ActionResult MainMenu()
        {
            List<Menus> list = menusDAO.getListByParentId(0, "MainMenu");
            return View("MainMenu", list);
        }
        ///////////////////////////////////////////////////////////////////////////
        // GET: MainmenuSub
        public ActionResult MainMenuSub(int id)
        {
            List<Menus> list = menusDAO.getListByParentId(id, "MainMenu");
            //Trả về dòng hiện hành của menu có id = id
            Menus menus = menusDAO.getRow(id);
            if (list.Count == 0)//menu không có cấp con
            {
                return View("MainMenuSub_0", menus);
            }
            else//menu có cấp con
            {
                ViewBag.Menu2cap = menus;
                return View("MainMenuSub_1", list);
            }
        }
        ///////////////////////////////////////////////////////////////////////////
        public ActionResult Slider()
        {
            SlidersDAO slidersDAO = new SlidersDAO();
            List<Sliders> list = slidersDAO.getListByPosition("Slider");
            return View("Slider", list);

        }
        ///////////////////////////////////////////////////////////////////////////
        public ActionResult CategoriesList()
        {
            CategoriesDAO categoriesDAO = new CategoriesDAO();
            List<Categories> list = categoriesDAO.getListByPareantId(0);
            return View("CategoriesList", list);

        }

        ///////////////////////////////////////////////////////////////////////////
        ///Footer Menu
        public ActionResult MenuFooter()
        {
            MenusDAO menusDAO = new MenusDAO();
            List<Menus> list = menusDAO.getListByParentId(0, "Footer");
            return View("MenuFooter", list);
        }

        // Action để hiển thị danh sách nhà cung cấp
        public ActionResult SuppliersList()
        {
            SuppliersDAO suppliersDAO = new SuppliersDAO();
            List<Suppliers> list = suppliersDAO.getList("Index");
            return View("SuppliersList", list);
        }
        ///////////////////////////////////////////////////////////////////////////
        public ActionResult PageList()
        {
            PostsDAO postsDAO = new PostsDAO();
            List<Posts> list = postsDAO.getList("Index","page");
            return View("PageList", list);
        }
    }
}