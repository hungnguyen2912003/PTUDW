using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClass.Model;

namespace MyClass.DAO
{
    public class ProductsDAO
    {
        private MyDBContext db = new MyDBContext();
        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách toàn bộ sản phẩm theo từng Categories (CatID)
        public List<Products> getListByCatId(int catid, int limit)
        {
            List<Products> list = db.Products
                .Where(m => m.CatID == catid && m.Status == 1)
                .Take(limit)
                .OrderByDescending(m => m.CreateBy)
                .ToList();
            return list;
        }
        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách toàn bộ sản phẩm theo từng Categories (CatID)
        //Dành cho trang HOME
        public List<ProductInfo> getListByListCatId(List<int> listcatid, int limit)
        {
            List<ProductInfo> list = db.Products
                .Join(
                    db.Categories, // Bảng Categories
                    p => p.CatID, // Khóa ngoại của Products liên kết với Categories
                    c => c.Id, // Khóa chính của Categories
                    (p, c) => new { Product = p, Category = c } // Kết hợp Products và Categories
                    )
                    .Join(
                        db.Suppliers, // Bảng Suppliers
                        pc => pc.Product.SupplierID, // Khóa ngoại của Product/Category liên kết với Suppliers
                        s => s.Id, // Khóa chính của Suppliers
                        (pc, s) => new ProductInfo
                        {
                            Id = pc.Product.Id,
                            CatID = pc.Product.CatID,
                            Name = pc.Product.Name,
                            CatName = pc.Category.Name, // Lấy tên danh mục từ bảng Categories
                            SupplierId = pc.Product.SupplierID,
                            SupplierName = s.Name, // Lấy tên nhà cung cấp từ bảng Suppliers
                            Slug = pc.Product.Slug,
                            Detail = pc.Product.Detail,
                            Image = pc.Product.Image,
                            Price = pc.Product.Price,
                            SalePrice = pc.Product.SalePrice,
                            Amount = pc.Product.Amount,
                            MetaDesc = pc.Product.MetaDesc,
                            MetaKey = pc.Product.MetaKey,
                            CreateBy = pc.Product.CreateBy,
                            CreateAt = pc.Product.CreateAt,
                            UpdateBy = pc.Product.UpdateBy,
                            UpdateAt = pc.Product.UpdateAt,
                            Status = pc.Product.Status
                        }
                     )
                .Where(m => m.Status == 1 && listcatid.Contains(m.CatID))
                .Take(limit)
                .OrderByDescending(m => m.CreateBy)
                .ToList();
            return list;
        }
        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách toàn bộ sản phẩm dựa vào Limit số mẩu tin
        //Dành cho trang Home
        public List<ProductInfo> getListBylimit(int limit)
        {
            List<ProductInfo> list = db.Products
                .Join(
                    db.Categories, // Bảng Categories
                    p => p.CatID, // Khóa ngoại của Products liên kết với Categories
                    c => c.Id, // Khóa chính của Categories
                    (p, c) => new { Product = p, Category = c } // Kết hợp Products và Categories
                    )
                    .Join(
                        db.Suppliers, // Bảng Suppliers
                        pc => pc.Product.SupplierID, // Khóa ngoại của Product/Category liên kết với Suppliers
                        s => s.Id, // Khóa chính của Suppliers
                        (pc, s) => new ProductInfo
                        {
                            Id = pc.Product.Id,
                            CatID = pc.Product.CatID,
                            Name = pc.Product.Name,
                            CatName = pc.Category.Name, // Lấy tên danh mục từ bảng Categories
                            SupplierId = pc.Product.SupplierID,
                            SupplierName = s.Name, // Lấy tên nhà cung cấp từ bảng Suppliers
                            Slug = pc.Product.Slug,
                            Detail = pc.Product.Detail,
                            Image = pc.Product.Image,
                            Price = pc.Product.Price,
                            SalePrice = pc.Product.SalePrice,
                            Amount = pc.Product.Amount,
                            MetaDesc = pc.Product.MetaDesc,
                            MetaKey = pc.Product.MetaKey,
                            CreateBy = pc.Product.CreateBy,
                            CreateAt = pc.Product.CreateAt,
                            UpdateBy = pc.Product.UpdateBy,
                            UpdateAt = pc.Product.UpdateAt,
                            Status = pc.Product.Status
                        }
                     )
                .Where(m => m.Status == 1)
                .Take(limit)
                .OrderByDescending(m => m.CreateBy)
                .ToList();
            return list;
        }

        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thi jdanh sách toàn bộ Loại sản phẩm: SELCT * FROM
        //Dành cho trang Quản trị
        public List<ProductInfo> getList(string status = "All")
        {
            List<ProductInfo> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Products
                            .Where(p => p.Status != 0)
                            .Join(
                                db.Categories, // Bảng Categories
                                p => p.CatID, // Khóa ngoại của Products liên kết với Categories
                                c => c.Id, // Khóa chính của Categories
                                (p, c) => new { Product = p, Category = c } // Kết hợp Products và Categories
                            )
                            .Join(
                                db.Suppliers, // Bảng Suppliers
                                pc => pc.Product.SupplierID, // Khóa ngoại của Product/Category liên kết với Suppliers
                                s => s.Id, // Khóa chính của Suppliers
                                (pc, s) => new ProductInfo
                                {
                                    Id = pc.Product.Id,
                                    CatID = pc.Product.CatID,
                                    Name = pc.Product.Name,
                                    CatName = pc.Category.Name, // Lấy tên danh mục từ bảng Categories
                                    SupplierId = pc.Product.SupplierID,
                                    SupplierName = s.Name, // Lấy tên nhà cung cấp từ bảng Suppliers
                                    Slug = pc.Product.Slug,
                                    Detail = pc.Product.Detail,
                                    Image = pc.Product.Image,
                                    Price = pc.Product.Price,
                                    SalePrice = pc.Product.SalePrice,
                                    Amount = pc.Product.Amount,
                                    MetaDesc = pc.Product.MetaDesc,
                                    MetaKey = pc.Product.MetaKey,
                                    CreateBy = pc.Product.CreateBy,
                                    CreateAt = pc.Product.CreateAt,
                                    UpdateBy = pc.Product.UpdateBy,
                                    UpdateAt = pc.Product.UpdateAt,
                                    Status = pc.Product.Status
                                }
                            )
                            .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Products
                            .Where(p => p.Status == 0)
                            .Join(
                                db.Categories, // Bảng Categories
                                p => p.CatID, // Khóa ngoại của Products liên kết với Categories
                                c => c.Id, // Khóa chính của Categories
                                (p, c) => new { Product = p, Category = c } // Kết hợp Products và Categories
                            )
                            .Join(
                                db.Suppliers, // Bảng Suppliers
                                pc => pc.Product.SupplierID, // Khóa ngoại của Product/Category liên kết với Suppliers
                                s => s.Id, // Khóa chính của Suppliers
                                (pc, s) => new ProductInfo
                                {
                                    Id = pc.Product.Id,
                                    CatID = pc.Product.CatID,
                                    Name = pc.Product.Name,
                                    CatName = pc.Category.Name, // Lấy tên danh mục từ bảng Categories
                                    SupplierId = pc.Product.SupplierID,
                                    SupplierName = s.Name, // Lấy tên nhà cung cấp từ bảng Suppliers
                                    Slug = pc.Product.Slug,
                                    Detail = pc.Product.Detail,
                                    Image = pc.Product.Image,
                                    Price = pc.Product.Price,
                                    SalePrice = pc.Product.SalePrice,
                                    Amount = pc.Product.Amount,
                                    MetaDesc = pc.Product.MetaDesc,
                                    MetaKey = pc.Product.MetaKey,
                                    CreateBy = pc.Product.CreateBy,
                                    CreateAt = pc.Product.CreateAt,
                                    UpdateBy = pc.Product.UpdateBy,
                                    UpdateAt = pc.Product.UpdateAt,
                                    Status = pc.Product.Status
                                }
                            )
                            .ToList();
                        break;
                    }
                // Các trường hợp khác xử lý tương tự
                default:
                    {
                        list = db.Products
                            .Join(
                                db.Categories,
                                p => p.CatID,
                                c => c.Id,
                                (p, c) => new { Product = p, Category = c }
                            )
                            .Join(
                                db.Suppliers,
                                pc => pc.Product.SupplierID,
                                s => s.Id,
                                (pc, s) => new ProductInfo
                                {
                                    Id = pc.Product.Id,
                                    CatID = pc.Product.CatID,
                                    Name = pc.Product.Name,
                                    CatName = pc.Category.Name,
                                    SupplierId = pc.Product.SupplierID,
                                    SupplierName = s.Name,
                                    Slug = pc.Product.Slug,
                                    Detail = pc.Product.Detail,
                                    Image = pc.Product.Image,
                                    Price = pc.Product.Price,
                                    SalePrice = pc.Product.SalePrice,
                                    Amount = pc.Product.Amount,
                                    MetaDesc = pc.Product.MetaDesc,
                                    MetaKey = pc.Product.MetaKey,
                                    CreateBy = pc.Product.CreateBy,
                                    CreateAt = pc.Product.CreateAt,
                                    UpdateBy = pc.Product.UpdateBy,
                                    UpdateAt = pc.Product.UpdateAt,
                                    Status = pc.Product.Status
                                }
                            )
                            .ToList();
                        break;
                    }
            }
            return list;
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ////Hiển thị danh sách 1 mẩu tin (bản ghi)
        public Products getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Products.Find(id);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách 1 mẩu tin (bản ghi) với kiểu string = slug
        public Products getRow(string slug)
        {

            return db.Products
                .Where(m => m.Slug == slug && m.Status == 1)
                .FirstOrDefault();

        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Thêm mới một mẩu tin
        public int Insert(Products row)
        {
            db.Products.Add(row);
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Cập nhật một mẩu tin
        public int Update(Products row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Xoá một mẩu tin ra khỏi CSDL
        public int Delete(Products row)
        {
            db.Products.Remove(row);
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Chi tiết sản phẩm
        ///Trang giao diện chi tiết sản phẩm
        public List<ProductInfo> GetProductDetailBySlug(string slug)
        {
            List<ProductInfo> list = null;
            list = db.Products
                .Where(p => p.Slug == slug && p.Status == 1)
                .Join(
                    db.Categories,
                    p => p.CatID,
                    c => c.Id,
                    (p, c) => new { Product = p, Category = c }
                )
                .Join(
                    db.Suppliers,
                    pc => pc.Product.SupplierID,
                    s => s.Id,
                    (pc, s) => new ProductInfo
                    {
                        Id = pc.Product.Id,
                        CatID = pc.Product.CatID,
                        Name = pc.Product.Name,
                        CatName = pc.Category.Name,
                        SupplierId = pc.Product.SupplierID,
                        SupplierName = s.Name,
                        Slug = pc.Product.Slug,
                        Detail = pc.Product.Detail,
                        Image = pc.Product.Image,
                        Price = pc.Product.Price,
                        SalePrice = pc.Product.SalePrice,
                        Amount = pc.Product.Amount,
                        MetaDesc = pc.Product.MetaDesc,
                        MetaKey = pc.Product.MetaKey,
                        CreateBy = pc.Product.CreateBy,
                        CreateAt = pc.Product.CreateAt,
                        UpdateBy = pc.Product.UpdateBy,
                        UpdateAt = pc.Product.UpdateAt,
                        Status = pc.Product.Status,
                        //bo sung truong Slug cua Categories
                        CategorySlug = pc.Category.Slug
                    }
                )
                .ToList();
            return list;
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Hiển thị danh sách các sản phẩm cùng loại
        ///Trang giao diện chi tiết sản phẩm
        public List<ProductInfo> GetProductDetailByCategoryId(int catid)
        {
            var relatedProducts = db.Products
                .Where(p => p.CatID == catid && p.Status == 1)
                .Join(
                    db.Categories,
                    p => p.CatID,
                    c => c.Id,
                    (p, c) => new { Product = p, Category = c }
                )
                .Join(
                    db.Suppliers,
                    pc => pc.Product.SupplierID,
                    s => s.Id,
                    (pc, s) => new ProductInfo
                    {
                        Id = pc.Product.Id,
                        CatID = pc.Product.CatID,
                        Name = pc.Product.Name,
                        CatName = pc.Category.Name,
                        SupplierId = pc.Product.SupplierID,
                        SupplierName = s.Name,
                        Slug = pc.Product.Slug,
                        Detail = pc.Product.Detail,
                        Image = pc.Product.Image,
                        Price = pc.Product.Price,
                        SalePrice = pc.Product.SalePrice,
                        Amount = pc.Product.Amount,
                        MetaDesc = pc.Product.MetaDesc,
                        MetaKey = pc.Product.MetaKey,
                        CreateBy = pc.Product.CreateBy,
                        CreateAt = pc.Product.CreateAt,
                        UpdateBy = pc.Product.UpdateBy,
                        UpdateAt = pc.Product.UpdateAt,
                        Status = pc.Product.Status
                    }
                )
                .ToList();
            return relatedProducts;
        }
    }
}
