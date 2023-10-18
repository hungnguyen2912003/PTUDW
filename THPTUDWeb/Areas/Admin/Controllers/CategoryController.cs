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

namespace THPTUDWeb.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Index
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"),"Id","Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //-----Create At
                categories.CreateAt = DateTime.Now;
                //-----Create By
                categories.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //-----ParentID
                if (categories.ParentID == null)
                {
                    categories.ParentID = 0;
                }
                //-----Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //-----UpdateAt
                categories.UpdateAt = DateTime.Now;
                //-----UpdateBy
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
                categoriesDAO.Insert(categories);
                //Hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Thêm loại sản phẩm mới thành công");
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            return View(categories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //-----Create At
                categories.CreateAt = DateTime.Now;
                //-----Create By
                categories.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //-----ParentID
                if (categories.ParentID == null)
                {
                    categories.ParentID = 0;
                }
                //-----Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //-----UpdateAt
                categories.UpdateAt = DateTime.Now;
                //-----UpdateBy
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
                categoriesDAO.Insert(categories);
                //Hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Sửa loại sản phẩm thành công");

                return RedirectToAction("Index");
            }
            return View(categories);
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            categoriesDAO.Delete(categories);
            return RedirectToAction("Index");
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index", "Category");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index", "Category");
            }
            //Cập nhật trạng thái
            categories.Status = (categories.Status == 1) ? 2 : 1;
            //Cập nhật UpdateBy
            categories.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            categories.UpdateAt = DateTime.Now;
            //Update Database
            categoriesDAO.Update(categories);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //Trở về trang Index
            return RedirectToAction("Index", "Category");
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá loại sản phẩm thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index", "Category");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá loại sản phẩm thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index", "Category");
            }
            //Cập nhật trạng thái
            categories.Status = 0;
            //Cập nhật UpdateBy
            categories.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            categories.UpdateAt = DateTime.Now;
            //Update Database
            categoriesDAO.Update(categories);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Xoá loại sản phẩm thành công");
            //Trở về trang Index
            return RedirectToAction("Index", "Category");
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Trash : Thùng rác
        public ActionResult Trash()
        {
            return View(categoriesDAO.getList("Trash"));
        }

    }
}
