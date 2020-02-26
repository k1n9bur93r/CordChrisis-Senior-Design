using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testServer.Models;

namespace testServer.DAOs
{
    public class MapStatsDA
    {
        public void Create(UserMapStats userMapStat)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                context.Add(userMapStat);
            }
        }
        public UserMapStats ReadSingle(string mapID, string userID)
        {
            UserMapStats data = new UserMapStats();
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                data = context.UserMapStats.Where(a=>(a.UserID == userID)&&(a.MapID==mapID)).FirstOrDefault();
            }
            return data;
        }

        public List<UserMapStats> ReadMapStatsByUser(string userID)
        {
            List<UserMapStats> data = new List<UserMapStats>();
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();

                data = context.UserMapStats.Where(a => a.UserID == userID).ToList();
            }
            return data;
        }

        public List<UserMapStats> ReadUserStatsByMap(string mapID)
        {
            List<UserMapStats> data = new List<UserMapStats>();
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();

                data = context.UserMapStats.Where(a => a.MapID == mapID).ToList();
            }
            return data;
        }

        public void Update(UserMapStats userMapStats)
        {
            using (var context = new ApplicationDBContext())
            {
                var row = context.UserMapStats.Where(a =>( a.UserID == userMapStats.UserID)&&(a.MapID==userMapStats.MapID)).FirstOrDefault();
                if (row == null) return;
                row = userMapStats;
                context.UserMapStats.Update(row);
                context.SaveChanges();
            }
        }

        public void Delete(string ID)
        {
            throw new NotImplementedException();
        }

    }
}
