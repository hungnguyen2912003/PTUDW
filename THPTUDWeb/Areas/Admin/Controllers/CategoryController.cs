﻿using System;
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
        LinksDAO linksDAO = new LinksDAO();
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Index
        //Trả về danh sách các mẩu tin
        public ActionResult Index()
        {
            //Hiển thị toàn bộ danh sách sản phẩm
            return View(categoriesDAO.getList("Index"));
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Details/5
        //Hiển thị một mẩu tin
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại sản phẩm");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại sản phẩm");
                return RedirectToAction("Index");
            }
            return View(categories);
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Create
        //Thêm mới một mẩu tin
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
                //-----Xử lý mục Topics
                if(categoriesDAO.Insert(categories) == 1)       //Nếu thêm dữ liệu thành công
                {
                    Links links = new Links();
                    links.Slug = categories.Slug;
                    links.TableID = categories.Id;
                    links.Type = "category";
                    linksDAO.Insert(links);
                }
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
        //Cập nhật một mẩu tin
        public ActionResult Edit(int? id)
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Chỉnh sửa loại sản phẩm thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Chỉnh sửa loại sản phẩm thất bại");
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
                //Cập nhật dữ liệu, sửa thêm phần Links phục vụ cho Topics
                //Nếu trùng khớp thông tin: Type = category & TableID = categories.ID
                Links links = linksDAO.getRow(categories.Id, "category");
                if(categoriesDAO.Update(categories) == 1)
                {
                    //Cập nhật dữ liệu
                    links.Slug = categories.Slug;
                    linksDAO.Update(links);
                }
                //Hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Chỉnh sửa loại sản phẩm thành công");

                return RedirectToAction("Index");
            }
            return View(categories);
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Delete/5
        //Xoá một mẩu tin ra khỏi CSDL
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá loại sản phẩm thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Trash");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá loại sản phẩm thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Trash");
            }
            return View(categories);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            //Tìm thấy mẩu tin => xoá, cập nhật cho Links
            if(categoriesDAO.Delete(categories) == 1)
            {
                Links links = linksDAO.getRow(categories.Id, "category");
                //Xoá luôn cho Links
                linksDAO.Delete(links);
            }
            //Hiển thị thông báo
            TempData["message"] = new XMessage("success", "Xoá loại sản phẩm thành công");
            return RedirectToAction("Trash");
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
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
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
                TempData["message"] = new XMessage("danger", "Xoá loại sản phẩm thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá loại sản phẩm thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Trash : Thùng rác
        //Hiển thị các mẩu tin có giá trị là 0
        public ActionResult Trash()
        {
            return View(categoriesDAO.getList("Trash"));
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Undo/5
        //Khôi phục mẩu tin. Chuyển trạng thái Status = 0 thành 2
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi loại sản phẩm thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi loại sản phẩm thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái status = 2
            categories.Status = 2;
            //Cập nhật UpdateBy
            categories.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            categories.UpdateAt = DateTime.Now;
            //Update Database
            categoriesDAO.Update(categories);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Phục hồi loại sản phẩm thành công");
            //Ở lại trang Trash để xoá tiếp
            return RedirectToAction("Trash");
        }
    }
}
