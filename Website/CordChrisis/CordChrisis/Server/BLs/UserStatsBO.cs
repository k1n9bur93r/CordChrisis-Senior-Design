using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using CordChrisis.BOs.Interfaces;
using CordChrisis.Shared.Models;
using DB;
using CordChrisis.DAOs;

namespace CordChrisis.BOs
{
    public class UserStatsBO : IUserStats
    {
        public Login LogInUser(Login login) {
            try
            {
                Console.WriteLine("were in the BO");
                UserLoginDA loginDb = new UserLoginDA();
             
                return loginDb.LoginUser(login);

            }
            catch (Exception ex)
            {
                Trace.TraceError("Error: " + ex.Message + "Unable to log in user! User" + login.Email);
                throw;
            }
        }

        public bool createUser(Login newUser)
        {
            try
            {
                UserDA creation = new UserDA();
                UserLoginInputDA createUser = new UserLoginInputDA();
                UserStatsDA newStats = new UserStatsDA();

                Guid id = Guid.NewGuid();

                newUser.ID = id;

                UserStats US = new UserStats
                {
                    ID = id.ToString(),
                    TotalMapsMade = 0,
                    HighestScore = 0,
                    MaxCombo = 0,
                    TotalScore = 0,
                    MostPlayedMap = "",
                    MapsCreated = 0,
                    TotalGamesPlayed = 0
                };


                User newU = new User
                {
                    ID = id.ToString(),
                    UserName = newUser.Email,
                    Rank = 0,
                    IsDeleted = false
                };

                bool check;

                //User Table
               check = creation.Create(newU);
                if (check == false)
                    return false; 

                //UserLogin table
              createUser.Create(newUser);
          


                //UserStats Table
              newStats.Create(US);
             

                return true; 

            }
            catch (Exception ex)
            {
                Trace.TraceError("Error: ");
                throw;
            }

        }

        public int GetSalt(SecureString password) {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error: " + ex.Message + "Unable to salt user information!: ");
                throw;
            }
        }

        public SecureString GetHash(SecureString password) 
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error: " + ex.Message + "Unable to hash user information!");
                throw;
            }
        }

        public void UpdateUserPassword(Login login) {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error: " + ex.Message + "Unable to update user password! " + login.Email);
                throw;
            }
        }


        public void UpdateUserPFP(byte[] pfp, string userID)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error: " + ex.Message + "Unable to update user PFP! User: " + userID);
                throw;
            }
        }

      

    }
}
