using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CordChrisis.Shared.Models;
using CordChrisis.BOs;

namespace CordChrisis.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserLoginController : Controller
    {
        [HttpPost]
        [Route("post")]
        public string GetLogin([FromBody] Login postData)
        {

            UserStatsBO log = new UserStatsBO();

            return log.LogInUser(postData);
        }
        //public IActionResult GetDefaultSearchList()
        //{
        //    return View();
        //}
    }
}

