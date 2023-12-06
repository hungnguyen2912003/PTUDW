using MyClass.DAO;
using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(Users users)
        {
            //So sánh thông tin người dùng
            Users row_user = usersDAO.getRow(users.Username, users.Password, "customer");
            if (row_user == null)
            {
                TempData["message"] = new XMessage("danger", "Đăng nhập thất bại do tên tài khoản hoặc mật khẩu không đúng!");
                return RedirectToAction("DangNhap");
            }
            else
            {
                Session["UserCustomer"] = row_user.Username;
                return RedirectToAction("Index", "Site");
            }
        }
        //////////////////////////////////////////////////////////////////////////
        // GET: Khachhang: Đăng ký
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(Users users)
        {
            if(ModelState.IsValid)
            {
                users.Role = "customer";
                users.CreateAt = DateTime.Now;
                users.CreateBy = Convert.ToInt32(Session["UserID"]);
                users.UpdateAt = DateTime.Now;
                users.UpdateBy = Convert.ToInt32(Session["UserID"]);
                users.Status = 1;
                if (usersDAO.UsernameValid(users.Username) && users.Role == "customer")
                {
                    ModelState.AddModelError("Username", "Tên người dùng đã tồn tại. Vui lòng chọn một tên khác.");
                    return View(users);
                }
                else
                {
                    usersDAO.Insert(users);
                    TempData["message"] = new XMessage("success", "Đăng ký thành công");
                    return RedirectToAction("DangKy");
                }
            }
            return View(users);
        }
        //////////////////////////////////////////////////////////////////////////
        ///Đăng xuất
        public ActionResult Dangxuat()
        {
            Session.Remove("UserCustomer");
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Site");
        }
    }
}