using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class SuppliersDAO
    {
        private MyDBContext db = new MyDBContext();
        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách toàn bộ Loại sản phẩm: SELCT * FROM
        public List<Suppliers> getList()
        {
            return db.Suppliers.ToList();
        }
        public List<Suppliers> getList(string status = "All")
        {
            List<Suppliers> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Suppliers
                            .Where(m => m.Status != 0)
                            .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Suppliers
                            .Where(m => m.Status == 0)
                            .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Suppliers.ToList();
                        break;
                    }
            }
            return list;
        }
        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách 1 mẩu tin (bản ghi)
        public Suppliers getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Suppliers.Find(id);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Thêm mới một mẩu tin
        public int Insert(Suppliers row)
        {
            db.Suppliers.Add(row);
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Cập nhật một mẩu tin
        public int Update(Suppliers row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Xoá một mẩu tin ra khỏi CSDL
        public int Delete(Suppliers row)
        {
            db.Suppliers.Remove(row);
            return db.SaveChanges();
        }
    }
}
