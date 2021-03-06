﻿using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CordChrisis.Shared.Models;

namespace CordChrisis.DAOs
{
    public class UserStatsDA
    {

        public void Create(UserStats user)
        {
            using (var context = new ApplicationDBContext())
            {
             
                context.Database.EnsureCreated();

                context.Add(user);
                context.SaveChanges();
              
            }
        }
        public UserStats ReadSingle(string userID)
        {
            UserStats data = new UserStats();
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                data = context.UserStats.Where(a => a.ID==userID).FirstOrDefault();
            }
            return data;

        }

        public void Update(UserStats userStats)
        {
            using (var context = new ApplicationDBContext())
            {
                var row = context.UserStats.Where(a => a.ID == userStats.ID).FirstOrDefault();
                if (row == null) return;
                row.UserImage = userStats.UserImage;
                row.ID = userStats.ID;
                row.TotalMapsMade = userStats.TotalMapsMade;
                row.HighestScore = userStats.HighestScore;
                row.MaxCombo = userStats.MaxCombo;
                row.TotalScore = userStats.TotalScore;
                row.MostPlayedMap = userStats.MostPlayedMap;
                row.MapsCreated = userStats.MapsCreated;
                row.TotalGamesPlayed = userStats.TotalGamesPlayed; 


                context.UserStats.Update(row);
                context.SaveChanges();
            }
        }


        public void Delete(string ID)
        {
            throw new NotImplementedException();
        }

    }
}
