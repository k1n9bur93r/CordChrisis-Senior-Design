using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Blazored.SessionStorage;

namespace CordChrisis.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBaseAddressHttpClient();
            builder.Services.AddBlazoredSessionStorage();
            //builder.Services.AddSingleton<CordChrisis.Client.Services.SessionStorage>();
            //builder.Services.AddSingleton<Blazored.SessionStorage.ISessionStorageService>();


            await builder.Build().RunAsync();
        }
    }
}
