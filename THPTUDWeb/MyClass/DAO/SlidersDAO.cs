using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class SlidersDAO
    {
        private MyDBContext db = new MyDBContext();
        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách toàn bộ Loại sản phẩm: SELCT * FROM
        public List<Sliders> getList(string status = "All")
        {
            List<Sliders> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Sliders
                        .Where(m => m.Status != 0)
                        .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Sliders
                        .Where(m => m.Status == 0)
                        .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Sliders.ToList();
                        break;
                    }
            }
            return list;
        }

        /////////////////////////////////////////////////////////////////////////////////////
        //Trả về 1 mẩu tin có diều kiện Pos = position và status = 1
        public List<Sliders> getListByPosition(string position)
        {
            return db.Sliders
              .Where(m => m.Position == position && m.Status == 1)
              .OrderBy(m => m.CreateAt)
              .ToList();
        }

        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách 1 mẩu tin (bản ghi)
        public Sliders getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Sliders.Find(id);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////
        ///Thêm một mẩu tin
        public int Insert(Sliders row)
        {
            db.Sliders.Add(row);
            return db.SaveChanges();
        }

        /////////////////////////////////////////////////////////////////////////////////////
        ///Cập nhật một mẩu tin
        public int Update(Sliders row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        /////////////////////////////////////////////////////////////////////////////////////
        ///Xoá một mẩu tin => xoá ra khỏi CSDL
        public int Delete(Sliders row)
        {
            db.Sliders.Remove(row);
            return db.SaveChanges();
        }
    }

}
