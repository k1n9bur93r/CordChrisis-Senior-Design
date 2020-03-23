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
    public class UserBO :IUserBO
    {
        public UserStats GetUserStats(string userID)
        {
			try
			{
				UserStatsDA user = new UserStatsDA();
				return user.ReadSingle(userID);
			}
			catch (Exception ex)
			{
				Trace.TraceError("Error: " + ex.Message + "Unable to get user stats for : " + userID);
				throw;
			}
		}

		public void PostUserStats(UserStats stats)
		{
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception ex)
			{
				Trace.TraceError("Error: " + ex.Message + "Unable to post user stats for user: " + stats.ID);
				throw;
			}
		}

		public UserMapStats GetUserMapStats(string userID, string mapID)
		{
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception ex)
			{
				Trace.TraceError("Error: " + ex.Message + "Unable to fetch UserMapStats for User : "+ userID +" and Map: " + mapID);
				throw;
			}
		}

		public void PostUserMapStats(UserMapStats uMapStats) {

			try
			{
				throw new NotImplementedException();
			}
			catch (Exception ex)
			{
				Trace.TraceError("Error: " + ex.Message + "Unable to post UserMapStats for User : "+uMapStats.UserID +" Map: " + uMapStats.MapID);
				throw;
			}
		}

    }
}
