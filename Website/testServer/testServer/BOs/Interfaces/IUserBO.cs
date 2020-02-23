using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testServer.Models;
namespace testServer.BOs.Interfaces
{
    public interface IUserBO
    {
        UserStats GetUserStats(string ID);
        void PostUserStats(UserStats stats);
        UserMapStats GetUserMapStats(string userID, string mapID);
        void PostUserMapStats(UserMapStats uMapStats);
    }
}
