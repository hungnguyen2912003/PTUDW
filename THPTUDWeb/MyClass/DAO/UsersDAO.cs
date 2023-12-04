using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class UsersDAO
    {
        private MyDBContext db = new MyDBContext();
        /////////////////////////////////////////////////////////////////////////////////////
        ///Hiển thị danh sách toàn bộ Người dùng: Select * FROM
        public List<Users> getList(string status = "All")
        {
            List<Users> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Users
                            .Where(m => m.Status != 0)
                     .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Users
                     .Where(m => m.Status == 0)
                     .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Users.ToList();
                        break;
                    }
            }
            return list;
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Hiển thị danh sách 1 mẩu tin (bản ghi)
        public Users getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Users.Find(id);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////
        ///Kiểm tra thông tin đăng nhập
        public Users getRow(string username, string password, string role)
        {
            return db.Users
               .Where(m => m.Status == 1 && m.Role == role && (m.Username == username && m.Password == password))
               .FirstOrDefault();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Thêm một mẩu tin
        public int Insert(Users row)
        {
            db.Users.Add(row);
            return db.SaveChanges();
        }

        /////////////////////////////////////////////////////////////////////////////////////
        ///Cập nhật một mẩu tin
        public int Update(Users row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        /////////////////////////////////////////////////////////////////////////////////////
        ///Xoá mọt mẩu tin ra khỏi CSDL
        public int Delete(Users row)
        {
            db.Users.Remove(row);
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Kiểm tra trùng tài khoản
        public bool UsernameValid(string username)
        {
            return db.Users.Any(m => m.Username == username);
        }
    }
}
