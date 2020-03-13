﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using CordChrisis.Shared.Models;

namespace CordChrisis.BOs.Interfaces
{
    public interface IUserStats
    {
        //Not too sure what we need to return here just yet...
        bool LogInUser(UserLogin login);

        int GetSalt(SecureString password);
        SecureString GetHash(SecureString password);
        void UpdateUserPassword(UserLogin login);
        void UpdateUserPFP(byte[] pfp,string userID);

    }
}