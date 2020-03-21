using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CordChrisis.Shared.Models;
using DB;
using CordChrisis.DAOs;

namespace CordChrisis.DAOs
{
    public class UserLoginDA
    {
        public Login LoginUser(Login log)
        {
            Login data = new Login();
            Login UserLogin = new Login();
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                UserLogin = context.UserLogin.Where(n => n.Email == log.Email && n.Password == log.Password).FirstOrDefault();
            }



            if (UserLogin == null)
            {
                return null;
            }
            else
            {
                UserLogin.Password = String.Empty;
                return UserLogin;
            }
        }

    }
}
