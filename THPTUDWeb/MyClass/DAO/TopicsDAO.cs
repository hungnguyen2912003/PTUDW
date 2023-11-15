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
    public class TopicsDAO
    {
        private MyDBContext db = new MyDBContext();

        //INDEX
        public List<Topics> getList()
        {
            return db.Topics.ToList();
        }

        //INDEX dựa vào Status = 1, 2, còn Status = 0 == Thùng rác
        public List<Topics> getList(string status = "All")
        {
            List<Topics> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Topics
                            .Where(m => m.Status != 0)
                            .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Topics
                            .Where(m => m.Status == 0)
                            .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Topics.ToList();
                        break;
                    }
            }
            return list;
        }

        //DETAILS
        public Topics getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Topics.Find(id);
            }
        }

        //CREATE
        public int Insert(Topics row)
        {
            db.Topics.Add(row);
            return db.SaveChanges();
        }

        //UPDATE
        public int Update(Topics row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //DELETE
        public int Delete(Topics row)
        {
            db.Topics.Remove(row);
            return db.SaveChanges();
        }
    }
}
