﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using testServer.BOs.Interfaces;
using testServer.Models;

namespace testServer.BOs
{
    public class UserStatsBO : IUserStats
    {
        public bool LogInUser(UserLogin login) {
            try
            {
                throw new NotImplementedException();
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

        public void UpdateUserPassword(UserLogin login) {
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
