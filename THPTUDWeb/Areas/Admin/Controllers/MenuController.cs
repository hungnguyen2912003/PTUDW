using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using THPTUDWeb.Library;

namespace THPTUDWeb.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {
        //Gọi 4 lớp DAO cần thực thi
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        SuppliersDAO suppliersDAO = new SuppliersDAO();
        ProductsDAO productsDAO = new ProductsDAO();
        MenusDAO menusDAO = new MenusDAO();
        TopicsDAO topicsDAO = new TopicsDAO();
        PostsDAO postsDAO = new PostsDAO();
        /// /////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Index
        public ActionResult Index()
        {
            //select * from Categories voi Status !=0
            ViewBag.CatList = categoriesDAO.getList("Index");
            //select * from Topics voi Status !=0
            ViewBag.TopList = topicsDAO.getList("Index");
            //select * from Posts voi Status !=0
            ViewBag.PostList = productsDAO.getList("Index");
            //select * from Suppliers voi Status !=0
            ViewBag.SupList = suppliersDAO.getList("Index");
            //Truyền menu dưới dạng Model
            return View(menusDAO.getList("Index"));
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            //-------------------------Category-----------------------//
            //--Xử lý nút ThemCategory bên Index
            if (!string.IsNullOrEmpty(form["ThemCategory"]))    //Nút ThemCategory được nhấn
            {
                //Check box được nhấn
                if (!string.IsNullOrEmpty(form["nameCategory"]))
                {
                    var listitem = form["nameCategory"];
                    //Chuyển danh sách thành dạng mảng: 1,2,3,4...
                    var listarr = listitem.Split(',');  //Ngắt theo dấu ","
                    foreach (var row in listarr)    //row = id của các mẩu tin
                    {
                        int id = int.Parse(row);    //ép kiểu int
                        //Lấy một bản ghi
                        Categories categories = categoriesDAO.getRow(id);
                        //Tạo ra Menu
                        Menus menu = new Menus();
                        menu.Name = categories.Name;
                        menu.Link = categories.Slug;
                        menu.TypeMenu = "category";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateAt = DateTime.Now;
                        menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.UpdateAt = DateTime.Now;
                        menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.Status = 2;    //Chưua xuất bản
                        //Thêm vào Database
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm vào menu danh mục loại sản phẩm thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục loại sản phẩm");
                }
            }
            //-------------------------Topic-----------------------//
            //--Xử lý các nút ThemTopic bên Index
            if (!string.IsNullOrEmpty(form["ThemTopic"]))//nút ThemCategory được nhấn
            {
                if (!string.IsNullOrEmpty(form["nameTopic"]))//check box được nhấn
                {
                    var listitem = form["nameTopic"];
                    //Chuyển danh sách thành dạng mảng: vi du 1,2,3,...
                    var listarr = listitem.Split(',');  //Ngắt theo dấu ","
                    foreach (var row in listarr)    //row = id của các mẩu tin
                    {
                        int id = int.Parse(row);    //Ép kiểu int
                        //Lấy một bản ghi
                        Topics topics = topicsDAO.getRow(id);
                        //Tạo ra Menu
                        Menus menu = new Menus();
                        menu.Name = topics.Name;
                        menu.Link = topics.Slug;
                        menu.TableID = topics.Id;
                        menu.TypeMenu = "topic";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateBy = Convert.ToInt32(Session["UserId"].ToString());
                        menu.CreateAt = DateTime.Now;
                        menu.Status = 2;    //Chưa xuất bản
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm vào menu danh mục chủ đề bài viết thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục chủ đề bài viết");
                }
            }
            //-------------------------Page-----------------------//
            //--Xử lý các nút Thempage bên Index
            if (!string.IsNullOrEmpty(form["ThemPage"]))
            {
                //Check box được nhấn từ phía Index
                if (!string.IsNullOrEmpty(form["namePage"]))
                {
                    var listitem = form["namePage"];
                    //Chuyển danh sách thành dạng mảng: 1,2,3,...
                    var listarr = listitem.Split(',');  //Ngắt theo dấu ","
                    foreach (var row in listarr)        //row = id của các mẩu tin
                    {
                        int id = int.Parse(row);    //Ép kiểu int
                        //Lấy một bản ghi
                        Posts post = postsDAO.getRow(id);
                        //tao ra menu
                        Menus menu = new Menus();
                        menu.Name = post.Title;
                        menu.Link = post.Slug;
                        menu.TableID = post.Id;
                        menu.TypeMenu = "page";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateBy = Convert.ToInt32(Session["UserId"].ToString());
                        menu.CreateAt = DateTime.Now;
                        menu.Status = 2;//chưa xuất bản
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm vào menu danh mục trang đơn thành công");
                }
                else//check box chưa được nhấn
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục trang đơn");
                }
            }
            //-------------------------Supplier-----------------------//
            //--Xử lý các nút ThemSupplier bên Index
            if (!string.IsNullOrEmpty(form["ThemSupplier"]))    //nút ThemSupplier được nhấn
            {
                if (!string.IsNullOrEmpty(form["nameSupplier"]))    //check box được nhấn
                {
                    var listitem = form["nameSupplier"];
                    //Chuyển danh sách thành dạng mảng: 1,2,3,4...
                    var listarr = listitem.Split(',');  //Ngắt theo dấu ","
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);    //ép kiểu int
                        //Lấy 1 bản ghi
                        Suppliers suppliers = suppliersDAO.getRow(id);
                        //Tạo ra Menu
                        Menus menu = new Menus();
                        menu.Name = suppliers.Name;
                        menu.Link = suppliers.Slug;
                        menu.TypeMenu = "supplier";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateAt = DateTime.Now;
                        menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.UpdateAt = DateTime.Now;
                        menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.Status = 2; //tam thoi chua xuat ban
                        //Them vao DB
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm vào menu danh mục nhà cung cấp thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn nhà cung cấp");
                }
            }
            //-------------------------Product-----------------------//
            //--Xử lý các nút ThemProduct bên Index
            if (!string.IsNullOrEmpty(form["ThemProduct"]))
            {
                //check box được nhấn
                if (!string.IsNullOrEmpty(form["nameProduct"]))
                {
                    var listitem = form["nameProduct"];
                    //Chuyển danh sách thành các mảng: 1,2,3,4...
                    var listarr = listitem.Split(',');  //Ngắt theo dấu ","
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);    //Ép kiểu int
                        //Lấy một bản ghi
                        Products products = productsDAO.getRow(id);
                        //Tạo ra menu
                        Menus menu = new Menus();
                        menu.Name = products.Name;
                        menu.Link = products.Slug;
                        menu.TypeMenu = "product";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateAt = DateTime.Now;
                        menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.UpdateAt = DateTime.Now;
                        menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.Status = 2;    //Chưa xuất bảng
                        //Thêm vào Database
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm vào menu danh mục sản phẩm thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn sản phẩm");
                }
            }
            //-------------------------Custom-----------------------//
            //--Xử lý các nút ThemCustom bên Index
            if (!string.IsNullOrEmpty(form["ThemCustom"]))
            {
                //Name, Link text được gõ từ phía Index
                if (!string.IsNullOrEmpty(form["nameCustom"]) && !string.IsNullOrEmpty(form["linkCustom"]))
                {
                    //Tạo ra Menu
                    Menus menu = new Menus();
                    menu.Name = form["nameCustom"]; //Lấy từ ô nhập dữ liệu (form)
                    menu.Link = form["linkCustom"]; //Lấy từ ô nhập dữ liệu (form)
                    menu.TypeMenu = "custom";
                    menu.Position = form["Position"];
                    menu.ParentID = 0;
                    menu.Order = 0;
                    menu.CreateAt = DateTime.Now;
                    menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                    menu.UpdateAt = DateTime.Now;
                    menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                    menu.Status = 2;    //Chưa xuất bản
                    //Thêm vào Database
                    menusDAO.Insert(menu);

                    TempData["message"] = new XMessage("success", "Thêm vào menu danh mục Custom thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa đầy đủ thông tin cho menu");
                }
            }
            //Trả về Index
            return RedirectToAction("Index", "Menu");
        }

        ////////////////////////////////////////////////////////////////
        //GET: Admin/Menu/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            else
            {
                //Chuyển đổi trạng thái Status
                menus.Status = (menus.Status == 1) ? 2 : 1;

                //Cập nhật giá trị UpdateAt
                menus.UpdateAt = DateTime.Now;

                //Cập nhật DB
                menusDAO.Update(menus);

                //Thông báo thành công
                TempData["message"] = TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");

                return RedirectToAction("Index");
            }
        }


        ////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy Menu");
                return RedirectToAction("Index");
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy Menu");
                return RedirectToAction("Index");
            }
            return View(menus);
        }
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Admin/Menu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Menus menus)
        {
            if (ModelState.IsValid)
            {
                menusDAO.Insert(menus);
                return RedirectToAction("Index");
            }

            return View(menus);
        }
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ParentList = new SelectList(menusDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(menusDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Chỉnh sửa Menu thất bại");
                return RedirectToAction("Index");
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                TempData["message"] = new XMessage("danger", "Chỉnh sửa Menu thất bại");
                return RedirectToAction("Index");
            }
            return View(menus);
        }
        // POST: Admin/Menu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menus menus)
        {
            if (ModelState.IsValid)
            {

                if (menus.ParentID == null)
                {
                    menus.ParentID = 0;
                }
                if (menus.Order == null)
                {
                    menus.Order = 1;
                }
                else
                {
                    menus.Order += 1;
                }

                //Xử lý cho mục UpdateAt
                menus.UpdateAt = DateTime.Now;

                //Xử lý cho mục UpdateBy
                menus.UpdateBy = Convert.ToInt32(Session["UserId"]);

                //Thông báo thành công
                TempData["message"] = new XMessage("success", "Chỉnh sửa Menu thành công");

                //Cập nhật Database
                menusDAO.Update(menus);

                return RedirectToAction("Index");
            }
            ViewBag.ParentList = new SelectList(menusDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(menusDAO.getList("Index"), "Order", "Name");
            return View(menus);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/DelTrash/5: Thay đổi trạng thái của mẩu tin = 0
        public ActionResult DelTrash(int? id)
        {
            //Khi nhập nút thay đổi Status cho một mẩu tin
            Menus menus = menusDAO.getRow(id);

            //Thay đổi trạng thái Status từ 1 hoặc 2 thành 0
            menus.Status = 0;

            //Cập nhật giá trị cho UpdateAt/By
            menus.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            menus.UpdateAt = DateTime.Now;

            //Gọi hàm Update trong MenusDAO
            menusDAO.Update(menus);

            //Thông báo thành công
            TempData["message"] = new XMessage("success", "Xóa Menu thành công");

            //Chuyển về Index
            return RedirectToAction("Index", "Menu");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menus/Trash/5: Hiển thị các mẩu tin có giá trị là 0
        public ActionResult Trash(int? id)
        {
            return View(menusDAO.getList("Trash"));
        }
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Recover/5: Thay đổi trạng thái của mẩu tin
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi Menu thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index", "Page");
            }
            //Khi nhấp nút thay đổi Status cho một mẩu tin
            Menus menus = menusDAO.getRow(id);
            //Kiểm tra id của menus có tồn tại?
            if (menus == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi Menu thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Thay đổi trạng thái Status = 2
            menus.Status = 2;
            //Cập nhật giá trị cho UpdateAt/By
            menus.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            menus.UpdateAt = DateTime.Now;
            //Gọi hàm Update trong menusDAO
            menusDAO.Update(menus);
            //Thông báo thành công
            TempData["message"] = new XMessage("success", "Phục hồi Menu thành công");
            //Chuyển về trang Trash để xoá tiếp
            return RedirectToAction("Trash");
        }
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá Menu thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Trash");
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá Menu thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Trash");
            }
            return View(menus);
        }
        // POST: Admin/Menu/Delete/5: Xoá một mẩu tin ra khỏi CSDL
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menus menus = menusDAO.getRow(id);
            //Tìm thấy mẩu tin thì xoá, cập nhật Links
            menusDAO.Delete(menus);
            //Thông báo thành công
            TempData["message"] = new XMessage("success", "Xóa Menu thành công");
            //Chuyển trở lại trang Trash
            return RedirectToAction("Trash");
        }
    }
}
