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
        public bool CreateUserController([FromBody] CreateAccount newAccount)
        {

           
            UserStatsBO user = new UserStatsBO();
            bool check = user.createUser(newAccount.loginObject, newAccount.username);

         
            return check; 
        }
    }
}