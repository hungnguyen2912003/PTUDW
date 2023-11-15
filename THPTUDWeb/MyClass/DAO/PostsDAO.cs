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

        //INDEX
        public List<Posts> getList()
        {
            return db.Posts.ToList();
        }

        //INDEX dựa vào Status = 1, 2, còn Status = 0 == Thùng rác
        public List<Posts> getList(string status = "All")
        {
            List<Posts> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Posts
                            .Where(m => m.Status != 0)
                            .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Posts
                            .Where(m => m.Status == 0)
                            .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Posts.ToList();
                        break;
                    }
            }
            return list;
        }

        //DETAILS
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

        //CREATE
        public int Insert(Posts row)
        {
            db.Posts.Add(row);
            return db.SaveChanges();
        }

        //UPDATE
        public int Update(Posts row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //DELETE
        public int Delete(Posts row)
        {
            db.Posts.Remove(row);
            return db.SaveChanges();
        }
    }
}
