using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CordChrisis.Shared.Models;
using CordChrisis.BOs;

namespace CordChrisis.Server.DALs
{
    [Route("[controller]")]
    [ApiController]
    public class CreateAccountController : Controller
    {
        [HttpPost]
        [Route("post")]
        public Login CreateUserController([FromBody] Login data)
        {

            Console.WriteLine("are we in the createaccoutn controller? ");
            UserStatsBO user = new UserStatsBO();
            user.createUser(data);

            Login succ = new Login();
            succ.Email = "test";
            succ.Password = "test";
            return succ; 
        }
    }
}