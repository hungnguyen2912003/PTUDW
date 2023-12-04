using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;
using MyClass.DAO;
using THPTUDWeb.Library;
using System.IO;

namespace THPTUDWeb.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        UsersDAO usersDAO = new UsersDAO();
        //////////////////////////////////////////////////////////////////////
        /////Trả về danh sách các mẩu tin
        // GET: Admin/Users
        public ActionResult Index()
        {
            return View(usersDAO.getList("Index"));
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Users/Details/5
        //Hiển thị một mẩu tin
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy người dùng");
                return RedirectToAction("Index");
            }
            Users users = usersDAO.getRow(id);
            if (users == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy người dùng");
                return RedirectToAction("Index");
            }
            return View(users);
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Users/Create
        //Thêm mới một mẩu tin
        public ActionResult Create()
        {
            return View();
        }
        // POST: Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users users)
        {
            if (ModelState.IsValid)
            {

                //Xử lý tự động cho các trường sau:
                //-----Create At
                users.CreateAt = DateTime.Now;
                //-----Create By
                users.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----UpdateAt
                users.UpdateAt = DateTime.Now;
                //-----UpdateBy
                users.UpdateBy = Convert.ToInt32(Session["UserID"]);
                if (usersDAO.UsernameValid(users.Username))
                {
                    ModelState.AddModelError("Username", "Tên người dùng đã tồn tại. Vui lòng chọn một tên khác.");
                    return View(users);
                }
                else
                {
                    usersDAO.Insert(users);
                    //Hiển thị thông báo thành công
                    TempData["message"] = new XMessage("success", "Thêm tài khoản người dùng thành công");
                    return RedirectToAction("Index");
                }
            }
            return View(users);
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Users/Edit/5
        //Cập nhật một mẩu tin
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy người dùng");
                return RedirectToAction("Index");
            }
            Users users = usersDAO.getRow(id);
            if (users == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy người dùng");
                return RedirectToAction("Index");
            }
            return View(users);
        }
        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users users)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //-----Create At
                users.CreateAt = DateTime.Now;
                //-----Create By
                users.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----UpdateAt
                users.UpdateAt = DateTime.Now;
                //-----UpdateBy
                users.UpdateBy = Convert.ToInt32(Session["UserID"]);
                usersDAO.Update(users);
                //Hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Chỉnh sửa tài khoản người dùng thành công");
                return RedirectToAction("Index");
            }
            return View(users);
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Users/Delete/5
        //Xoá một mẩu tin ra khỏi CSDL
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá tài khoản thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Trash");
            }
            Users users = usersDAO.getRow(id);
            if (users == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá tài khoản thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Trash");
            }
            return View(users);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = usersDAO.getRow(id);
            usersDAO.Delete(users);
            //Hiển thị thông báo
            TempData["message"] = new XMessage("success", "Xoá tài khoản thành công");
            return RedirectToAction("Trash");
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Users/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Users users = usersDAO.getRow(id);
            if (users == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái
            users.Status = (users.Status == 1) ? 2 : 1;
            //Cập nhật UpdateBy
            users.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            users.UpdateAt = DateTime.Now;
            //Update Database
            usersDAO.Update(users);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //Trở về trang Index
            return RedirectToAction("Index");
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/DelTrash/5
        //Thay đổi trạng thái của mẩu tin = 0 => Trash
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá tài khoản thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Users users = usersDAO.getRow(id);
            if (users == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá tài khoản thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái
            users.Status = 0;
            //Cập nhật UpdateBy
            users.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            users.UpdateAt = DateTime.Now;
            //Update Database
            usersDAO.Update(users);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Xoá tài khoản thành công");
            //Trở về trang Index
            return RedirectToAction("Index");
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Users/Trash : Thùng rác
        //Hiển thị các mẩu tin có giá trị là 0
        public ActionResult Trash()
        {
            return View(usersDAO.getList("Trash"));
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Users/Undo/5
        //Khôi phục mẩu tin. Chuyển trạng thái Status = 0 thành 2
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi tài khoản thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Users users = usersDAO.getRow(id);
            if (users == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi tài khoản thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái status = 2
            users.Status = 2;
            //Cập nhật UpdateBy
            users.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            users.UpdateAt = DateTime.Now;
            //Update Database
            usersDAO.Update(users);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Phục hồi tài khoản thành công");
            //Ở lại trang Trash để xoá tiếp
            return RedirectToAction("Trash");
        }
    }
}
