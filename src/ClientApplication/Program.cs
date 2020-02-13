using System.Threading.Tasks;
using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using Amolenk.ServerlessPonies.ClientApplication.Scenes;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApplication
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            
            builder.RootComponents.Add<App>("app");

            builder.Services
                .AddTransient<IPhaserInterop, PhaserInterop>()
                .AddTransient<ApiClient>()
                .AddTransient<BootScene>()
                .AddTransient<CreditsScene>()
                .AddTransient<RanchScene>()
                .AddTransient<AnimalManagementScene>()
                .AddTransient<AnimalPurchaseScene>()
                .AddTransient<AnimalSelectionScene>()
                .AddTransient<AnimalCareScene>()
                .AddTransient<SpinnerScene>();

            return builder.Build().RunAsync();
        }

        public static WebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
            WebAssemblyHostBuilder.CreateDefault();
    }
}
