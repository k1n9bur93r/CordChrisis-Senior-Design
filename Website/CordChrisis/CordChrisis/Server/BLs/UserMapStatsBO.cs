using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CordChrisis.BOs.Interfaces;
using CordChrisis.DAOs;
using CordChrisis.Shared.Models;


namespace CordChrisis.BOs
{
    public class UserMapStatsBO : IUserMapStats
    {
        public List<UserMapStats> GetMapHighScores(string MapID)
        {
            MapStatsDA mapStatsDA = new MapStatsDA();
            return mapStatsDA.ReadUserStatsByMapHighScores(MapID);
        }
    }
}
