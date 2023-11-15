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
    public class PostController : Controller
    {
        PostsDAO postsDAO = new PostsDAO();
        LinksDAO linksDAO = new LinksDAO();
        TopicsDAO topicsDAO = new TopicsDAO();
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Post/Index
        public ActionResult Index()
        {
            return View(postsDAO.getList("Index"));
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy bài viết");
                return RedirectToAction("Index");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy bài viết");
                return RedirectToAction("Index");
            }
            return View(posts);
        }

        // GET: Admin/Post/Create
        public ActionResult Create()
        {
            ViewBag.TopicList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Posts posts)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //-----Create At
                posts.CreateAt = DateTime.Now;
                //-----Create By
                posts.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----Slug
                posts.Slug = XString.Str_Slug(posts.Title);
                //-----UpdateAt
                posts.UpdateAt = DateTime.Now;
                //-----UpdateBy
                posts.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Xử lý thông tin phần Hình ảnh
                var img = Request.Files["img"];//Lấy thông tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//Lấy phần mở rộng tập tin
                    {
                        string slug = posts.Slug;
                        //Tên file = Slug + Phần mở rộng của tập tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        posts.Image = imgName;
                        //Upload hình
                        string PathDir = "~/Public/img/post/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//Kết thúc upload hình ảnh
                //-----Xử lý mục PostType
                posts.PostType = "post";
                //-----Xử lý mục Topics
                if (postsDAO.Insert(posts) == 1)       //Nếu thêm dữ liệu thành công
                {
                    Links links = new Links();
                    links.Slug = posts.Slug;
                    links.TableID = posts.Id;
                    links.Type = "post";
                    linksDAO.Insert(links);
                }
                //Hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Thêm loại bài viết thành công");
                return RedirectToAction("Index");
            }
            ViewBag.TopicList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            return View(posts);
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Post/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.TopicList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            return View(posts);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Posts posts)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //-----Create At
                posts.CreateAt = DateTime.Now;
                //-----Create By
                posts.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----Slug
                posts.Slug = XString.Str_Slug(posts.Title);
                //-----UpdateAt
                posts.UpdateAt = DateTime.Now;
                //-----UpdateBy
                posts.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //-----Xử lý mục PostType
                //posts.PostType = "post";
                //Xử lý thông tin phần Hình ảnh
                var img = Request.Files["img"];//Lấy thông tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//Lấy phần mở rộng tập tin
                    {
                        string slug = posts.Slug;
                        //Tên file = Slug + Phần mở rộng của tập tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        posts.Image = imgName;
                        //Upload hình
                        string PathDir = "~/Public/img/post/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        //Update thì phải xoá ảnh cũ
                        if (posts.Image != null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), posts.Image);
                            System.IO.File.Delete(DelPath);
                        }
                        img.SaveAs(PathFile);
                    }
                }//Kết thúc upload hình ảnh
                //Cập nhật dữ liệu, sửa thêm phần Links phục vụ cho Topics
                //Nếu trùng khớp thông tin: Type = category & TableID = posts.ID
                Links links = linksDAO.getRow(posts.Id, "post");
                if (postsDAO.Update(posts) == 1)
                {
                    //Cập nhật dữ liệu
                    links.Slug = posts.Slug;
                    links.TableID = posts.Id;
                    links.Type = "post";
                    linksDAO.Update(links);
                }
                //Hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Sửa bài viết thành công");

                return RedirectToAction("Index");
            }
            return View(posts);
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá mẩu tin thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Trash");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá mẩu tin thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Trash");
            }
            return View(posts);
        }

        // POST: Admin/Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posts posts = postsDAO.getRow(id);
            //Tìm thấy mẩu tin => xoá, cập nhật cho Links
            if (postsDAO.Delete(posts) == 1)
            {
                Links links = linksDAO.getRow(posts.Id, "post");
                //Xoá luôn cho Links
                linksDAO.Delete(links);
                //Đường dẫn đế nảnh cần xoá
                string PathDir = "~/Public/img/post/";
                //Cập nhật, xoá file cũ
                if(posts.Image != null)
                {
                    string DelPath = Path.Combine(Server.MapPath(PathDir), posts.Image);
                    System.IO.File.Delete(DelPath);
                }
            }
            //Hiển thị thông báo
            TempData["message"] = new XMessage("success", "Xoá mẩu tin thành công");
            return RedirectToAction("Trash");
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Post/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái
            posts.Status = (posts.Status == 1) ? 2 : 1;
            //Cập nhật UpdateBy
            posts.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            posts.UpdateAt = DateTime.Now;
            //Update Database
            postsDAO.Update(posts);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //Trở về trang Index
            return RedirectToAction("Index");
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Post/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá bài viết thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá bài viết thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái
            posts.Status = 0;
            //Cập nhật UpdateBy
            posts.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            posts.UpdateAt = DateTime.Now;
            //Update Database
            postsDAO.Update(posts);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Xoá bài viết thành công");
            //Trở về trang Index
            return RedirectToAction("Index");
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Post/Trash : Thùng rác
        public ActionResult Trash()
        {
            return View(postsDAO.getList("Trash"));
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Post/Undo/5
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái status = 2
            posts.Status = 2;
            //Cập nhật UpdateBy
            posts.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            posts.UpdateAt = DateTime.Now;
            //Update Database
            postsDAO.Update(posts);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Phục hồi mẩu tin thành công");
            //Ở lại trang Trash để xoá tiếp
            return RedirectToAction("Trash");
        }
    }
}
