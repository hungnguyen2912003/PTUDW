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
    public class LinksDAO
    {
        private MyDBContext db = new MyDBContext();

        //INDEX
        public List<Links> getList()
        {
            return db.Links.ToList();
        }

        //INDEX dựa vào Status = 1, 2, còn Status = 0 == Thùng rác
        public List<Links> getList(string status = "All")
        {
            List<Links> list = null;
            return list;
        }

        //Hiển thị danh sách 1 mẩu tin
        public Links getRow(int tableid, string typelink)
        {
            return db.Links
                .Where(model => model.TableID == tableid && model.Type == typelink)
                .FirstOrDefault();
        }

        //Thêm một mẩu tin mới
        public int Insert(Links row)
        {
            db.Links.Add(row);
            return db.SaveChanges();
        }

        //Cập nhật mẩu tin
        public int Update(Links row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Xoá mẩu tin ra khỏi DB
        public int Delete(Links row)
        {
            db.Links.Remove(row);
            return db.SaveChanges();
        }
    }
}
