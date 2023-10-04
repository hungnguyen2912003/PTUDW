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
				categories.CreateAt = DateTime.Now;
				categories.UpdateAt = DateTime.Now;
				//Xu ly cho muc CreateBy
				categories.CreateBy = Convert.ToInt32(Session["UserId"]);
				//Xy ly cho muc UpdateBy
				categories.UpdateBy = Convert.ToInt32(Session["UserId"]);
				categoriesDAO.Insert(categories);
				return RedirectToAction("Index");
			}

			return View(categories);
		}

		//---------------------------------------------------------------------------------------------

		// GET: Admin/Categories/Edit/5
		public ActionResult Edit(int? id)
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

		// POST: Admin/Categories/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Categories categories)
		{
			if (ModelState.IsValid)
			{
				categoriesDAO.Update(categories);
				return RedirectToAction("Index");
			}
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
	}
}
