using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THPTUDWeb.Library
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public int Ammount { get; set; }
        public decimal Total { get; set; }
        public CartItem()
        {

        }
        public CartItem(int proid, string name, string img, decimal price, decimal saleprice, int qty)
        {
            this.ProductId = proid;
            this.Name = name;
            this.Img = img;
            this.Price = price;
            this.SalePrice = saleprice;
            this.Ammount = qty;
            this.Total = saleprice * qty;
        }
    }
}