using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using Amolenk.ServerlessPonies.ClientApplication.Scenes;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApplication
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args);

            builder.Services
                .AddTransient<IPhaserInterop, PhaserInterop>()
                .AddTransient<ApiClient>()
                .AddTransient<BootScene>()
                .AddTransient<CreditsScene>()
                .AddTransient<RanchScene>()
                .AddTransient<AnimalManagementScene>()
                .AddTransient<AnimalCareScene>()
                .AddTransient<SpinnerScene>();

            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "FunctionsBaseUrl", "https://serverlessponies.azurewebsites.net/" }
               //{ "FunctionsBaseUrl", "http://localhost:7071/" }
            });

            builder.RootComponents.Add<App>("app");

            return builder.Build().RunAsync();
        }

        public static WebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
            WebAssemblyHostBuilder.CreateDefault();
    }
}
