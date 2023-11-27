using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class PostsDAO
    {
        private MyDBContext db = new MyDBContext();

        public List<Posts> getList()
        {
            return db.Posts.ToList();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách toàn bộ Loại sản phẩm: SELCT * FROM
        public List<Posts> getList(string status = "All", string type = "Post")
        {
            List<Posts> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Posts
                        .Where(m => m.Status != 0 && m.PostType == type)
                        .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Posts
                        .Where(m => m.Status == 0 && m.PostType == type)
                        .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Posts
                            .Where(m => m.PostType == type)
                            .ToList();
                        break;
                    }
            }
            return list;
        }
        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách 1 mẩu tin (bản ghi)
        public Posts getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Posts.Find(id);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách 1 mẩu tin (bản ghi) với kiểu string = slug
        public Posts getRow(string slug)
        {
            return db.Posts
                    .Where(m => m.Slug == slug && m.Status == 1)
                    .FirstOrDefault();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Thêm mới một mẩu tin
        public int Insert(Posts row)
        {
            db.Posts.Add(row);
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Cập nhật một mẩu tin
        public int Update(Posts row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Xoá một mẩu tin ra khỏi CSDL
        public int Delete(Posts row)
        {
            db.Posts.Remove(row);
            return db.SaveChanges();
        }
    }
}
