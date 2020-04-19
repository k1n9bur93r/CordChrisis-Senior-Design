using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CordChrisis.Shared.Models;

namespace CordChrisis.BOs.Interfaces
{
    public interface IUserMapStats
    {
        List<UserMapStats> GetMapHighScores(string MapID);
    }
}
