using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CordChrisis.BOs;
using CordChrisis.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CordChrisis.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {

  

        [HttpPost]
        [Route("post")]
        public User getUserInfo([FromBody] string UserID)
        {
            User tempdata = new User();
            UserStatsBO userStatsBO = new UserStatsBO();
            tempdata=userStatsBO.GetUser(UserID);
            return tempdata;
        }
    }
}