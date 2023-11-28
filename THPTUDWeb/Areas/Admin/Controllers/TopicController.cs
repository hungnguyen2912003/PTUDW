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
    public class TopicController : Controller
    {
        TopicsDAO topicsDAO = new TopicsDAO();
        LinksDAO linksDAO = new LinksDAO();
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Index: Trả về danh sách các mẩu tin
        public ActionResult Index()
        {
            //Hiển thị toàn bộ danh sách các chủ đề
            return View(topicsDAO.getList("Index"));
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Details/5: Hiển thị thông tin một mẩu tin
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy chủ đề bài viết");
                return RedirectToAction("Index");
            }
            Topics topics = topicsDAO.getRow(id);
            if (topics == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy chủ đề bài viết");
                return RedirectToAction("Index");
            }
            return View(topics);
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.TopicList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(topicsDAO.getList("Index"), "Order", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Topics topics)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //-----Create At
                topics.CreateAt = DateTime.Now;
                //-----Create By
                topics.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----Slug
                topics.Slug = XString.Str_Slug(topics.Name);
                //-----ParentID
                if (topics.ParentID == null)
                {
                    topics.ParentID = 0;
                }
                //-----Order
                if (topics.Order == null)
                {
                    topics.Order = 1;
                }
                else
                {
                    topics.Order += 1;
                }
                //-----UpdateAt
                topics.UpdateAt = DateTime.Now;
                //-----UpdateBy
                topics.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //-----Xử lý mục Topics
                if (topicsDAO.Insert(topics) == 1)       //Nếu thêm dữ liệu thành công
                {
                    Links links = new Links();
                    links.Slug = topics.Slug;
                    links.TableID = topics.Id;
                    links.Type = "topic";
                    linksDAO.Insert(links);
                }
                //Hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Thêm chủ đề bài viết mới thành công");
                return RedirectToAction("Index");
            }
            ViewBag.TopicList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(topicsDAO.getList("Index"), "Order", "Name");
            return View(topics);
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.TopicList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(topicsDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Topics topics = topicsDAO.getRow(id);
            if (topics == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            return View(topics);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Topics topics)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //-----Create At
                topics.CreateAt = DateTime.Now;
                //-----Create By
                topics.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----Slug
                topics.Slug = XString.Str_Slug(topics.Name);
                //-----ParentID
                if (topics.ParentID == null)
                {
                    topics.ParentID = 0;
                }
                //-----Order
                if (topics.Order == null)
                {
                    topics.Order = 1;
                }
                else
                {
                    topics.Order += 1;
                }
                //-----UpdateAt
                topics.UpdateAt = DateTime.Now;
                //-----UpdateBy
                topics.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Cập nhật dữ liệu, sửa thêm phần Links phục vụ cho Topics
                //Nếu trùng khớp thông tin: Type = category & TableID = topics.ID
                Links links = linksDAO.getRow(topics.Id, "category");
                if (topicsDAO.Update(topics) == 1)
                {
                    //Cập nhật dữ liệu
                    links.Slug = topics.Slug;
                    linksDAO.Update(links);
                }
                //Hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Sửa chủ đề bài viết thành công");

                return RedirectToAction("Index");
            }
            return View(topics);
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá chủ đề bài viết thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Trash");
            }
            Topics topics = topicsDAO.getRow(id);
            if (topics == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá chủ đề bài viết thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Trash");
            }
            return View(topics);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Topics topics = topicsDAO.getRow(id);
            //Tìm thấy mẩu tin => xoá, cập nhật cho Links
            if (topicsDAO.Delete(topics) == 1)
            {
                Links links = linksDAO.getRow(topics.Id, "topic");
                //Xoá luôn cho Links
                linksDAO.Delete(links);
            }
            //Hiển thị thông báo
            TempData["message"] = new XMessage("success", "Xoá chủ dề bài viết thành công");
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
            Topics topics = topicsDAO.getRow(id);
            if (topics == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái
            topics.Status = (topics.Status == 1) ? 2 : 1;
            //Cập nhật UpdateBy
            topics.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            topics.UpdateAt = DateTime.Now;
            //Update Database
            topicsDAO.Update(topics);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //Trở về trang Index
            return RedirectToAction("Index");
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá chủ đề bài viết thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Topics topics = topicsDAO.getRow(id);
            if (topics == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá chủ đề bài viết thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái
            topics.Status = 0;
            //Cập nhật UpdateBy
            topics.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            topics.UpdateAt = DateTime.Now;
            //Update Database
            topicsDAO.Update(topics);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Xoá chủ đề bài viết thành công");
            //Trở về trang Index
            return RedirectToAction("Index");
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Trash : Thùng rác
        public ActionResult Trash()
        {
            return View(topicsDAO.getList("Trash"));
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Undo/5
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi chủ đề bài viết thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Topics topics = topicsDAO.getRow(id);
            if (topics == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi chủ đề bài viết thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái status = 2
            topics.Status = 2;
            //Cập nhật UpdateBy
            topics.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            topics.UpdateAt = DateTime.Now;
            //Update Database
            topicsDAO.Update(topics);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Phục hồi chủ đề bài viết thành công");
            //Ở lại trang Trash để xoá tiếp
            return RedirectToAction("Trash");
        }
    }
}
