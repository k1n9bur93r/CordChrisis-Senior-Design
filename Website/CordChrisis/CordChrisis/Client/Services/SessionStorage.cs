using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CordChrisis.Client.Services
{
    public class SessionStorage
    {
        private const string SessionKey = "LoggedIn";

        private readonly ISessionStorageService _sessStore;

        //public SessionStorage(NavigationManager nav, ISessionStorageService Store)
        //{
        //    _navMan = nav;
        //    _sessStore = Store;
        //}

        public async Task<string> GetUserLoginAsync()
        {
            return await _sessStore.GetItemAsync<string>(SessionKey);
        }
        public async Task InsertUserLoginAsync(string ID)
        {
            await _sessStore.SetItemAsync(SessionKey, ID);
        }

    }
}
