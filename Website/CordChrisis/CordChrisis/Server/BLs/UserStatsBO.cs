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
        public string LogInUser(Login login) {
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
