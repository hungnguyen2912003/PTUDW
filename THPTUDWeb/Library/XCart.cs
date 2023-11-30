using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using THPTUDWeb.Library;

namespace THPTUDWeb.Library
{
    public class XCart
    {
        List<CartItem> list = new List<CartItem>();
        public List<CartItem> AddCart(CartItem cartitem, int productid)
        {
            if (System.Web.HttpContext.Current.Session["MyCart"].Equals(""))    //session chưa có giỏ hàng
            {
                List<CartItem> list = new List<CartItem>();
                list.Add(cartitem);
                System.Web.HttpContext.Current.Session["MyCart"] = list;
            }
            else
            {
                //Đã có thông tin trong giỏ hàng, lấy thông tin của session -> Ép kiểu về list
                List<CartItem> list = (List<CartItem>)System.Web.HttpContext.Current.Session["MyCart"];
                //Kiểm tra productid đã có trong danh sách hay chưa?
                int count = list.Where(m => m.ProductId == productid).Count();
                if (count > 0)  //Đã có trong danh sách giỏ hàng trước đó
                {
                    cartitem.Ammount += 1;
                    //Cập nhật lại danh sách
                    int vt = 0;
                    foreach (var item in list)
                    {
                        if (item.ProductId == productid)
                        {
                            list[vt].Ammount += 1;
                            list[vt].Total = list[vt].Ammount * list[vt].SalePrice;
                        }
                        vt++;
                    }
                    System.Web.HttpContext.Current.Session["MyCart"] = list;
                }
                else
                {
                    //Thêm vào giỏ hàng mới
                    list.Add(cartitem);
                    System.Web.HttpContext.Current.Session["MyCart"] = list;
                }
            }
            return list;
        }

        //////////////////////////////////////////////////////////////////
        ///UpdateCart
        public void UpdateCart(string[] arramout)
        {
            //Đã có thông tin trong giỏ hàng, lấy thông tin của session - > ép kiểu về list
            List<CartItem> list = this.GetCart();
            int vt = 0;
            foreach (CartItem cartitem in list)
            {
                list[vt].Ammount = int.Parse(arramout[vt]);
                list[vt].Total = list[vt].Ammount * list[vt].SalePrice;
                vt++;
            }
            //Cập nhật lại giỏ hàng
            System.Web.HttpContext.Current.Session["MyCart"] = list;
        }
        //////////////////////////////////////////////////////////////////
        ///DelCart
        public void DelCart(int? productid = null)
        {
            if (productid != null)
            {
                if (!System.Web.HttpContext.Current.Session["MyCart"].Equals(""))
                {
                    List<CartItem> list = (List<CartItem>)System.Web.HttpContext.Current.Session["MyCart"];
                    int vt = 0;
                    foreach (var item in list)
                    {
                        if (item.ProductId == productid)
                        {
                            list.RemoveAt(vt);
                            break;
                        }
                        vt++;
                    }
                    //Cập nhật lại giỏ hàng
                    System.Web.HttpContext.Current.Session["MyCart"] = list;
                }
            }
            else
            {
                //Cập nhật lại giỏ hàng
                System.Web.HttpContext.Current.Session["MyCart"] = "";
            }
        }
        //////////////////////////////////////////////////////////////////
        ///GetCart
        public List<CartItem> GetCart()
        {
            if (System.Web.HttpContext.Current.Session["MyCart"].Equals(""))
            {
                return null;
            }
            return (List<CartItem>)System.Web.HttpContext.Current.Session["MyCart"];
        }
    }
}