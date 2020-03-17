using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CordChrisis.Shared.Models;

namespace CordChrisis.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserLoginController : Controller
    {
        [HttpPost]
        [Route("post")]
        public Boolean GetLogin([FromBody] Login postData)
        {


            Console.WriteLine("are we gettting here?");
            Console.WriteLine("\n\n\n\n\n\n\n");


            return  true;
        }
        //public IActionResult GetDefaultSearchList()
        //{
        //    return View();
        //}
    }
}