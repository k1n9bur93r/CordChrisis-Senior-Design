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
    public class UserMapStatsController : Controller
    {


        [Route("post")]
        [HttpPost]
        public void SaveUserMapStats([FromBody]UserMapStats stats)
        {
            UserBO userBO = new UserBO();
            userBO.PostUserMapStats(stats);
        }
        [Route("get")]
        [HttpPost]
        public UserMapStats GetUserMapStats([FromBody]UserMapStats stats)
        {
            UserBO userBO = new UserBO();
            
            stats=userBO.GetUserMapStats(stats.UserID, stats.MapID);
            if (stats == null) stats = new UserMapStats();
            return stats;
            //return userBO.GetUserMapStats(stats.MapID,stats.UserID);
        }

        [Route("gethighscores")]
        [HttpPost]
        public List<UserMapStats> GetUserMapStatsForMap([FromBody]string MapID)
        {
            List<UserMapStats> returnData = new List<UserMapStats>();
            UserMapStatsBO userMapStatsBO = new UserMapStatsBO();
            returnData = userMapStatsBO.GetMapHighScores(MapID);
            return returnData;
        }
    }
}