using MyClass.DAO;
using MyClass.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THPTUDWeb.Library;

namespace THPTUDWeb.Controllers
{
    public class GiohangController : Controller
    {
        ProductsDAO productsDAO = new ProductsDAO();
        XCart xcart = new XCart();
        // GET: Cart
        public ActionResult Index()
        {
            //Đã có thông tin trong giỏ hàng, lấy thông tin của session -> Ép kiểu về list
            List<CartItem> list = xcart.GetCart();
            return View("Index", list);

        }
        //////////////////////////////////////////////////////////////////
        ///Thêm vào giỏ hàng
        public ActionResult AddCart(int productid)
        {
            Products products = productsDAO.getRow(productid);
            CartItem cartitem = new CartItem(products.Id, products.Name, products.Image, products.Price, 1);
            //Thêm vào giỏ hàng với danh sách list phần tử = Session = MyCart
            if (Session["MyCart"].Equals(""))   //session chưa có giỏ hàng
            {
                List<CartItem> list = new List<CartItem>();
                list.Add(cartitem);
                Session["MyCart"] = list;
            }
            else
            {
                //Đã có thông tin trong giỏ hàng, lấy thông tin của session -> Ép kiểu về list 
                List<CartItem> list = (List<CartItem>)Session["MyCart"];
                //Kiểm tra productid đã có trong danh sách hay chưa
                int count = list.Where(m => m.ProductId == productid).Count();
                if (count > 0)  //đã có trong danh sách giỏ hàng trước đó
                {
                    cartitem.Ammount += 1;
                    //Cập nhật lại danh sách
                    int vt = 0;
                    foreach (var item in list)
                    {
                        if (item.ProductId == productid)
                        {
                            list[vt].Ammount += 1;
                        }
                        vt++;
                    }
                    Session["MyCart"] = list;
                }
                else
                {
                    //Thêm vào giỏ hàng mới
                    list.Add(cartitem);
                    Session["MyCart"] = list;
                }
            }
            //Chuyển hướng trang
            return RedirectToAction("Index", "Giohang");
        }


        //////////////////////////////////////////////////////////////////
        ///DelCart
        public ActionResult DelCart(int productid)
        {
            xcart.DelCart(productid);
            return RedirectToAction("Index", "Giohang");
        }

        //////////////////////////////////////////////////////////////////
        ///CartUpdate
        public ActionResult CartUpdate(FormCollection form)
        {
            if (!string.IsNullOrEmpty(form["capnhat"])) //nút ThemCategory được nhấn
            {
                var listamount = form["amount"];
                //Chuyển danh sách thì dạng mảng: vi du 1,2,3,...
                var listarr = listamount.Split(',');    //Ngắt theo dấu ","
                xcart.UpdateCart(listarr);
            }
            return RedirectToAction("Index", "Giohang");
        }

        //////////////////////////////////////////////////////////////////
        ///CartUpdate
        public ActionResult CartDelAll()
        {
            xcart.DelCart();
            return RedirectToAction("Index", "Giohang");
        }

        //////////////////////////////////////////////////////////////////
        ///ThanhToan
        public ActionResult ThanhToan()
        {
            //Kiểm tra thông tin đăng nhập trang người dùng = Khách hàng
            if (Session["UserCustomer"].Equals(""))
            {
                return Redirect("~/dang-nhap"); //Chuyển hướng đến URL
            }
            return View("ThanhToan");
        }
    }
}