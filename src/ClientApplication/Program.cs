using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using Amolenk.ServerlessPonies.ClientApplication.Scenes;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApplication
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services
                .AddBaseAddressHttpClient()
                .AddTransient<IPhaserInterop, PhaserInterop>()
                .AddTransient<ApiClient>()
                .AddTransient<BootScene>()
                .AddTransient<CreditsScene>()
                .AddTransient<RanchScene>()
                .AddTransient<AnimalManagementScene>()
                .AddTransient<AnimalCareScene>();

            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "FunctionsBaseUrl", "https://serverlessponies9321.azurewebsites.net/" }
                //{ "FunctionsBaseUrl", "http://localhost:7071/" }
            });

            return builder.Build().RunAsync();
        }
    }
}
