using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class MenusDAO
    {
        private MyDBContext db = new MyDBContext();

        //SELECT * FROM ...
        public List<Menus> getList()
        {
            return db.Menus.ToList();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách toàn bộ Loại sản phẩm: SELCT * FROM    
        public List<Menus> getList(string status = "All")
        {
            List<Menus> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Menus
                        .Where(m => m.Status != 0)
                        .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Menus
                        .Where(m => m.Status == 0)
                        .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Menus.ToList();
                        break;
                    }
            }
            return list;
        }
        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách 1 mẩu tin (bản ghi)
        public Menus getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Menus.Find(id);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Thêm mới một mẩu tin
        public int Insert(Menus row)
        {
            db.Menus.Add(row);
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Cập nhật một mẩu tin
        public int Update(Menus row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Xoá một mẩu tin ra khỏi CSDL
        public int Delete(Menus row)
        {
            db.Menus.Remove(row);
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách thoả 2 điều kiện cho trang người dùng
        public List<Menus> getListByParentId(int parentid, string position)
        {
            return db.Menus
                    .Where(m => m.ParentID == parentid && m.Status == 1 && m.Position == position)
                    .OrderBy(m => m.Order)
                    .ToList();
        }
    }
}