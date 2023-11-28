using MyClass.DAO;
using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace THPTUDWeb.Controllers
{
    public class SiteController : Controller
    {
        LinksDAO linksDAO = new LinksDAO();
        // GET: Site
        public ActionResult Index(string slug = null)
        {
            if (slug == null)
            {
                //Chuyền về trang chủ
                return this.Home();
            }
            else
            {
                //Tìm slug có trong bảng Link
                Links links = linksDAO.getRow(slug);
                //Kiểm tra slug có tồn tại trong bảng Links hay không?
                if (links != null)
                {
                    //Lấy ra Type trong bảng Link
                    string typelink = links.Type;
                    switch (typelink)
                    {
                        case "category":
                            {
                                return this.ProductCategory(slug);
                            }
                        case "topic":
                            {
                                return this.PostTopic(slug);
                            }
                        case "page":
                            {
                                return this.PostPage(slug);
                            }
                        default:
                            {
                                return this.Error404(slug);
                            }
                    }
                }
                else
                {
                    //Slug không có trong bảng Links
                    //slug có trong bảng product?
                    //slug có trong bảng Post với PostType==post?
                    ProductsDAO productsDAO = new ProductsDAO();
                    PostsDAO postsDAO = new PostsDAO();

                    //Tìm slug có trong bảng Products
                    Products products = productsDAO.getRow(slug);
                    if (products != null)
                    {
                        return this.ProductDetail(slug);
                    }
                    else
                    {
                        //Trường hợp là Post
                        Posts posts = postsDAO.getRow(slug);
                        if (posts != null)
                        {
                            return this.PostDetail(posts);
                        }
                        else
                        {
                            return this.Error404(slug);
                        }
                    }
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////
        ///TRUY VẤN DỮ LIỆU TỪ MỤC CATEGORY ĐỂ ĐIỀN VÀO CHO PHẦN DANH MỤC
        public ActionResult CategoriesList()
        {
            CategoriesDAO categoriesDAO = new CategoriesDAO();
            List<Categories> list = categoriesDAO.getListByPareantId(0);
            return View("CategoriesList", list);

        }
        /////////////////////////////////////////////////////////////////////////////
        //Trang chu
        public ActionResult Home()
        {
            CategoriesDAO categoriesDAO = new CategoriesDAO();
            List<Categories> list = categoriesDAO.getListByPareantId(0);
            return View("Home", list);
        }
        /////////////////////////////////////////////////////////////////////////////
        //Site/Product
        //HIỂN THỊ TẤT CẢ SẢN PHẨM KHÔNG PHÂN BIỆT
        public ActionResult Product()
        {
            ProductsDAO productsDAO = new ProductsDAO();
            List<ProductInfo> list = productsDAO.getListBylimit(10);
            return View("Product", list);
        }
        public ActionResult Post()
        {
            return View("Post");
        }
        /////////////////////////////////////////////////////////////////////////////
        //Site/ProductCategory
        public ActionResult ProductCategory(string slug)
        {
            //Lấy categories đưa vào Slug
            CategoriesDAO categoriesDAO = new CategoriesDAO();
            Categories categories = categoriesDAO.getRow(slug);
            //Hiển thị nội dung của mẩu tin
            ViewBag.Categories = categories;
            //Hiển thị toàn bộ các sản phẩm ứng với từng loại sản phẩm
            //Hiển thị theo 3 cấp: Cha - Con - Con của con
            List<int> listcatid = new List<int>();
            //Cấp 1
            listcatid.Add(categories.Id);
            //Cấp 2
            List<Categories> listcategories2 = categoriesDAO.getListByPareantId(categories.Id);
            if (listcategories2.Count() != 0)
            {
                foreach (var categories2 in listcategories2)
                {
                    listcatid.Add(categories2.Id);
                    //Cấp 3
                    List<Categories> listcategories3 = categoriesDAO.getListByPareantId(categories2.Id);
                    if (listcategories3.Count() != 0)
                    {
                        foreach (var categories3 in listcategories3)
                        {
                            listcatid.Add(categories3.Id);
                        }
                    }
                }
            }
            ProductsDAO productsDAO = new ProductsDAO();
            List<ProductInfo> list = productsDAO.getListByListCatId(listcatid, 10);
            return View("ProductCategory", list);
        }
        /////////////////////////////////////////////////////////////////////////////
        //Site/PostTopic
        public ActionResult PostTopic(string slug)
        {
            return View("PostTopic");
        }
        /////////////////////////////////////////////////////////////////////////////
        //Site/PostPage
        public ActionResult PostPage(string slug)
        {
            PostsDAO postsDAO = new PostsDAO();
            Posts posts = postsDAO.getRow(slug);
            return View("PostPage", posts);
        }
        /////////////////////////////////////////////////////////////////////////////
        //Site/Error404
        public ActionResult Error404(string slug)
        {
            return View("Error404");
        }
        /////////////////////////////////////////////////////////////////////////////
        //Product/Details
        public ActionResult ProductDetail(string slug)
        {
            //Hiển thị nộki dung của sản phẩm
            ProductsDAO productsDAO = new ProductsDAO();
            List<ProductInfo> list = productsDAO.GetProductDetailBySlug(slug);
            //Lấy CatID của sản phẩm hiện tại
            CategoriesDAO categoriesDAO = new CategoriesDAO();
            int catid = list.First().CatID;
            //Lấy danh sách sản phẩm cùng loại (related products)
            List<ProductInfo> relatedProducts = productsDAO.GetProductDetailByCategoryId(catid);
            //Truyền danh sách sản phẩm cùng loại cho View
            ViewBag.RelatedProducts = relatedProducts;
            //Trả về danh sách chi tiết sản phẩm cho List
            return View("ProductDetail", list);
        }
        /////////////////////////////////////////////////////////////////////////////
        //Post/Details
        public ActionResult PostDetail(Posts posts)
        {
            return View("PostDetail");
        }
        /////////////////////////////////////////////////////////////////////////////
        //HomeProduct
        public ActionResult HomeProduct(int id)
        {
            CategoriesDAO categoriesDAO = new CategoriesDAO();
            Categories categories = categoriesDAO.getRow(id);
            ViewBag.Categories = categories;
            //Hiển thị toàn bộ các sản phẩm ứng với từng loại sản phẩm
            //Hiển thị the 3 cấp: Cha - Con - Con của con
            List<int> listcatid = new List<int>();
            //Cấp 1
            listcatid.Add(id);
            //Cấp 2
            List<Categories> listcategories2 = categoriesDAO.getListByPareantId(id);
            if (listcategories2.Count() != 0)
            {
                foreach (var categories2 in listcategories2)
                {
                    listcatid.Add(categories2.Id);
                    //Cấp 3
                    List<Categories> listcategories3 = categoriesDAO.getListByPareantId(categories2.Id);
                    if (listcategories3.Count() != 0)
                    {
                        foreach (var categories3 in listcategories3)
                        {
                            listcatid.Add(categories3.Id);
                        }
                    }
                }
            }
            ProductsDAO productsDAO = new ProductsDAO();
            List<ProductInfo> list = productsDAO.getListByListCatId(listcatid, 10);
            return View("HomeProduct", list);

        }
    }
}