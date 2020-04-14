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
    public class UserStatsController : Controller
    {
        [HttpPost]
        [Route("post")]
        public UserStats GetUserStats([FromBody]string Id)
        {
            UserBO userStats = new UserBO();

            UserStats returnData = userStats.GetUserStats(Id);
            if (returnData == null)
            {
                returnData = new UserStats { ID = "NO DATA" };

            }
            return returnData;
        }

        [HttpPost]
        [Route("picture")]
        public UserStats AddNewPFP([FromBody]UserStats pic)
        {

            UserBO userStats = new UserBO();
            userStats.AddUserImage(pic);


            return null;

        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}