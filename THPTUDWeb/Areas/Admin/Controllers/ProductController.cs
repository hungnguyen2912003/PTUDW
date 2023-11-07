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
using System.IO;    //Upload hình ảnh
namespace THPTUDWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProductsDAO productDAO = new ProductsDAO();
        SuppliersDAO suppliersDAO = new SuppliersDAO();
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/product/Index
        public ActionResult Index()
        {
            return View(productDAO.getList("Index"));
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại sản phẩm");
                return RedirectToAction("Index");
            }
            Products products = productDAO.getRow(id);
            if (products == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại sản phẩm");
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // GET: Admin/product/Create
        public ActionResult Create()
        {
            ViewBag.ListSupList = new SelectList(suppliersDAO.getList("Index"), "Id", "Name");
            ViewBag.ListCatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //-----Create At
                products.CreateAt = DateTime.Now;
                //-----Create By
                products.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----Slug
                products.Slug = XString.Str_Slug(products.Name);
                //-----UpdateAt
                products.UpdateAt = DateTime.Now;
                //-----UpdateBy
                products.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Xử lý thông tin phần Hình ảnh
                var img = Request.Files["img"];//Lấy thông tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//Lấy phần mở rộng tập tin
                    {
                        string slug = products.Slug;
                        //Tên file = Slug + Phần mở rộng của tập tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Image = imgName;
                        //Upload hình
                        string PathDir = "~/Public/img/product/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//Kết thúc upload hình ảnh
                productDAO.Insert(products);
                //Hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Thêm loại sản phẩm mới thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListSupList = new SelectList(suppliersDAO.getList("Index"), "Id", "Name");
            ViewBag.ListCatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            return View(products);
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/product/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.CatList = new SelectList(productDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(productDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Products products = productDAO.getRow(id);
            if (products == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            return View(products);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //-----Create At
                products.CreateAt = DateTime.Now;
                //-----Create By
                products.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----Slug
                products.Slug = XString.Str_Slug(products.Name);
                //-----UpdateAt
                products.UpdateAt = DateTime.Now;
                //-----UpdateBy
                products.UpdateBy = Convert.ToInt32(Session["UserID"]);
                productDAO.Insert(products);
                //Hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Sửa loại sản phẩm thành công");

                return RedirectToAction("Index");
            }
            return View(products);
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá mẩu tin thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Trash");
            }
            Products products = productDAO.getRow(id);
            if (products == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá mẩu tin thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Trash");
            }
            return View(products);
        }

        // POST: Admin/product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = productDAO.getRow(id);
            //Tìm thấy mẩu tin => xoá
            productDAO.Delete(products);
            //Hiển thị thông báo
            TempData["message"] = new XMessage("success", "Xoá mẩu tin thành công");
            return RedirectToAction("Trash");
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/product/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Products products = productDAO.getRow(id);
            if (products == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái
            products.Status = (products.Status == 1) ? 2 : 1;
            //Cập nhật UpdateBy
            products.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            products.UpdateAt = DateTime.Now;
            //Update Database
            productDAO.Update(products);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //Trở về trang Index
            return RedirectToAction("Index");
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/product/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá loại sản phẩm thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Products products = productDAO.getRow(id);
            if (products == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá loại sản phẩm thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái
            products.Status = 0;
            //Cập nhật UpdateBy
            products.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            products.UpdateAt = DateTime.Now;
            //Update Database
            productDAO.Update(products);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Xoá loại sản phẩm thành công");
            //Trở về trang Index
            return RedirectToAction("Index");
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/product/Trash : Thùng rác
        public ActionResult Trash()
        {
            return View(productDAO.getList("Trash"));
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/product/Undo/5
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Products products = productDAO.getRow(id);
            if (products == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái status = 2
            products.Status = 2;
            //Cập nhật UpdateBy
            products.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            products.UpdateAt = DateTime.Now;
            //Update Database
            productDAO.Update(products);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Phục hồi mẩu tin thành công");
            //Ở lại trang Trash để xoá tiếp
            return RedirectToAction("Trash");
        }
    }
}
