using MyClass.DAO;
using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THPTUDWeb.Library;

namespace THPTUDWeb.Controllers
{
    public class KhachhangController : Controller
    {
        // GET: Khachhang
        UsersDAO usersDAO = new UsersDAO();
        public ActionResult Index()
        {
            return View();
        }
        //////////////////////////////////////////////////////////////////////////
        // GET: Khachhang: Đăng nhập
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection field)
        {
            String username = field["username "];
            String password = XString.ToMD5(field["password "]);
            //So sánh thông tin người dùng
            Users row_user = usersDAO.getRow(username, "customer");
            String strErr = "";
            if (row_user == null)
            {
                strErr = "Tên đăng nhập không tồn tại";
            }
            ViewBag.Error = "<span class='text-danger'>" + strErr + "</div";
            return View("DangNhap");
        }
        //////////////////////////////////////////////////////////////////////////
        // GET: Khachhang: Đăng ký
        public ActionResult DangKy()
        {
            return View();
        }

    }
}