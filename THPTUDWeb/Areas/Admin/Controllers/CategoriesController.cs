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
    public class CategoriesController : Controller
    {
		CategoriesDAO categoriesDAO = new CategoriesDAO();
        //---------------------------------------------------------------------------------------------
        // GET: Admin/Categories/Index
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }

		//---------------------------------------------------------------------------------------------

		// GET: Admin/Categories/Details/5
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

		////---------------------------------------------------------------------------------------------

		// GET: Admin/Categories/Create
		public ActionResult Create()
		{
			ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
			ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
			return View();
		}

		// POST: Admin/Categories/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Categories categories)
		{
			if (ModelState.IsValid)
			{
				//Xu ly cho muc Slug
				categories.Slug = Xstring.Str_Slug(categories.Name);
				//Xu ly cho muc ParentID
				if (categories.ParentID == null)
				{
					categories.ParentID = 0;
				}
				//Xu ly cho muc Order
				if (categories.Order == null)
				{
					categories.Order = 1;
				}
				else
				{
					categories.Order = categories.Order + 1;
				}
				//Xu ly cho muc CreateAt
				categories.CreateAt = DateTime.Now;
				//Xu ly cho muc CreateBy
				categories.CreateBy = Convert.ToInt32(Session["UserId"]);
				//Xy ly cho muc UpdateAt
				categories.UpdateAt = DateTime.Now;
				//Xy ly cho muc UpdateBy
				categories.UpdateBy = Convert.ToInt32(Session["UserId"]);
				categoriesDAO.Insert(categories);
				return RedirectToAction("Index");
			}
			ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
			ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
			return View(categories);
		}

		//---------------------------------------------------------------------------------------------

		// GET: Admin/Categories/Edit/5: Cap nhat mau tin
		public ActionResult Edit(int? id)
		{
			ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
			ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
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

		// POST: Admin/Categories/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Categories categories)
		{
			if (ModelState.IsValid)
			{
				//Xu ly cho muc Slug
				categories.Slug = Xstring.Str_Slug(categories.Name);
				//chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

				//Xu ly cho muc ParentId
				if (categories.ParentID == null)
				{
					categories.ParentID = 0;
				}

				//Xu ly cho muc Order
				if (categories.Order == null)
				{
					categories.Order = 1;
				}
				else
				{
					categories.Order = categories.Order + 1;
				}

				//Xu ly cho muc CreateAt
				categories.CreateAt = DateTime.Now;

				//Xu ly cho muc CreateBy
				categories.CreateBy = Convert.ToInt32(Session["UserId"]);

				//Xy ly cho muc UpdateAt
				categories.UpdateAt = DateTime.Now;

				//Xy ly cho muc UpdateBy
				categories.UpdateBy = Convert.ToInt32(Session["UserId"]);

				//Thong bao thanh cong
				TempData["message"] = new XMessage("success", "Thêm danh mục thành công");

				//Cap nhat du lieu 
				categoriesDAO.Update(categories);
				return RedirectToAction("Index");
			}
			ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
			ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
			return View(categories);
		}

		////---------------------------------------------------------------------------------------------

		// GET: Admin/Categories/Delete/5
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

		// POST: Admin/Categories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Categories categories = categoriesDAO.getRow(id);
			categoriesDAO.Delete(categories);
			return RedirectToAction("Index");
		}
		/////////////////////////////////////////////////////////////////////////////////////
		// GET: Admin/Category/Staus/5:Thay doi trang thai cua mau tin
		public ActionResult Status(int? id)
		{
			if (id == null)
			{
				//Thong bao that bai
				TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
				//chuyen huong trang
				return RedirectToAction("Index", "Categories");
			}
			//khi nhap nut thay doi Status cho mot mau tin
			Categories categories = categoriesDAO.getRow(id);
			//Kiem tra id cua categories co ton tai?
			if(categories == null)
			{
				//Thong bao that bai
				TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
				//Thong bao thanh cong
				TempData["message"] = new XMessage("success", "Thêm danh mục thành công");
				//Chuyen huong trang
				return RedirectToAction("Index", "Categories");
			}
			//thay doi trang thai Status tu 1 thanh 2 va nguoc lai
			categories.Status = (categories.Status == 1) ? 2 : 1;
			//cap nhat gia tri cho UpdateAt/By
			categories.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
			categories.UpdateAt = DateTime.Now;
			//Goi ham Update trong CategoryDAO
			categoriesDAO.Update(categories);
			//Thong bao thanh cong
			TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
			//khi cap nhat xong thi chuyen ve Index
			return RedirectToAction("Index", "Categories");
		}
		/////////////////////////////////////////////////////////////////////////////////////
		// GET: Admin/Category/DelTrash/5:Thay doi trang thai cua mau tin = 0
		public ActionResult DelTrash(int? id)
		{
			//khi nhap nut thay doi Status cho mot mau tin
			Categories categories = categoriesDAO.getRow(id);

			//thay doi trang thai Status tu 1,2 thanh 0
			categories.Status = 0;

			//cap nhat gia tri cho UpdateAt/By
			categories.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
			categories.UpdateAt = DateTime.Now;

			//Goi ham Update trong CategoryDAO
			categoriesDAO.Update(categories);

			//Thong bao thanh cong
			TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");

			//khi cap nhat xong thi chuyen ve Index
			return RedirectToAction("Index", "Category");
		}

	}
}
