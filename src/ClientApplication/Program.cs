using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using Amolenk.ServerlessPonies.ClientApplication.Scenes;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Amolenk.ServerlessPonies.ClientApplication;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApplication
{
    public class Program
    {
        private const string ENVIRONMENT_SUFFIX = "9321";

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
                { "FunctionsBaseUrl", $"https://serverlessponies{ENVIRONMENT_SUFFIX}.azurewebsites.net/" }
            });

            return builder.Build().RunAsync();
        }
    }
}
