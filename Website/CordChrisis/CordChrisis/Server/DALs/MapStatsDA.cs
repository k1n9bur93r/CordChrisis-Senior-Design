using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CordChrisis.Shared.Models;

namespace CordChrisis.DAOs
{
    public class MapStatsDA
    {
        public void Create(UserMapStats userMapStat)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                context.Add(userMapStat);
                context.SaveChanges();
            }
        }
        public UserMapStats ReadSingle(string mapID, string userID)
        {
            UserMapStats data = new UserMapStats();
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                data = context.MapStats.Where(a=>(a.UserID == userID)&&(a.MapID==mapID)).FirstOrDefault();
            }
            return data;
        }

        public List<UserMapStats> ReadMapStatsByUser(string userID)
        {
            List<UserMapStats> data = new List<UserMapStats>();
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();

                data = context.MapStats.Where(a => a.UserID == userID).ToList();
            }
            return data;
        }

        public List<UserMapStats> ReadUserStatsByMap(string mapID)
        {
            List<UserMapStats> data = new List<UserMapStats>();
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();

                data = context.MapStats.Where(a => a.MapID == mapID).ToList();
            }
            return data;
        }

        public List<UserMapStats> ReadUserStatsByMapHighScores(string mapID)
        {
            List<UserMapStats> data = new List<UserMapStats>();
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();

                data = context.MapStats.Where(a => a.MapID == mapID).OrderByDescending(a=>a.Score).Take(15).ToList();
            }
            return data;
        }

        public void Update(UserMapStats userMapStats)
        {
            using (var context = new ApplicationDBContext())
            {
                var row = context.MapStats.Where(a =>( a.UserID == userMapStats.UserID)&&(a.MapID==userMapStats.MapID)).FirstOrDefault();
                if (row == null) return;
                row.Plays += row.Plays;
                row.Rating = userMapStats.Rating;
                row.LetterScore = userMapStats.LetterScore;
                row.MaxCombo = userMapStats.MaxCombo > row.MaxCombo ? userMapStats.MaxCombo : row.MaxCombo; //are we setting the higest over all or the higest for this particualr thing? 
                row.Score = userMapStats.Score > row.Score ? userMapStats.Score : row.Score; //are we setting the higest over all or the higest for this particualr thing? 

                context.MapStats.Update(row);
                context.SaveChanges();
            }
        }

        public void Delete(string ID)
        {
            throw new NotImplementedException();
        }

    }
}
