using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CordChrisis.Shared.Models;
using DB;

namespace CordChrisis.DAOs
{
    public class UserDA
    {
        public bool Create(User newuser)
        {

            User data = new User();
            using (var context = new ApplicationDBContext()) 
            {
                context.Database.EnsureCreated();
                data = context.Users.Where(a => a.UserName == newuser.UserName).FirstOrDefault();
                if (data != null)
                {
                    return false;
                }
                else
                {
                    context.Users.Add(newuser);
                    context.SaveChanges();
                }
                return true;
              
            }
        }
        public User ReadSingle(string userID)
        {
            User data = new User();
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                data = context.Users.Where(a=> (a.ID == userID)&&(a.IsDeleted==false)).FirstOrDefault();
            }
            return data;
        }

        public void Update(User user)
        {
            using (var context = new ApplicationDBContext())
            {
                var row = context.Users.Where(a => a.ID == user.ID).FirstOrDefault();
                if (row == null) return;
                row = user;
                context.Users.Update(row);
                context.SaveChanges();
            }
        }

        public void Delete(string ID)
        {
            using (var context = new ApplicationDBContext())
            {
                var row = context.Users.Where(a => a.ID == ID).FirstOrDefault();
                if (row == null) return;
                row.IsDeleted=true;
                context.Users.Update(row);
                context.SaveChanges();
            }
        }


    }
}
