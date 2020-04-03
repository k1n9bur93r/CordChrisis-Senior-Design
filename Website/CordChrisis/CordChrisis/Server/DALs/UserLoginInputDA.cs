using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CordChrisis.Shared.Models;
using DB;
using CordChrisis.DAOs;

namespace CordChrisis.DAOs
{
    public class UserLoginInputDA
    {
        public void Create(Login log)
        {

            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();

                context.UserLogin.Add(log);
                context.SaveChanges();

            }
          
            
        }

    }
}
