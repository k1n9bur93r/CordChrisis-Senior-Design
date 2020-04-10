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
				MapStatsDA mapStatsDA = new MapStatsDA();
				return mapStatsDA.ReadSingle(mapID,userID);
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
				MapStatsDA mapStatsDA = new MapStatsDA();
				if (mapStatsDA.ReadSingle(uMapStats.MapID,uMapStats.UserID) == null)
				{
					mapStatsDA.Create(uMapStats);
				}
				else 
				{
					mapStatsDA.Update(uMapStats);
				}

			}
			catch (Exception ex)
			{
				Trace.TraceError("Error: " + ex.Message + "Unable to post UserMapStats for User : "+uMapStats.UserID +" Map: " + uMapStats.MapID);
				throw;
			}
		}

		public void AddUserImage(UserStats pfpuser)
		{
			try
			{
				UserStatsDA user = new UserStatsDA();
				user.Update(pfpuser);
			}
			catch (Exception ex)
			{
				Trace.TraceError("Error: " + ex.Message + "Unable to Get ProfilePicture for User");
				throw;
			}
		}

    }
}
