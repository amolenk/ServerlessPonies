using Blazor.Extensions;
using ClientApplication.Scenes;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApplication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Use Blazor Extensions to wrap the SignalR Typescript client.
            // https://github.com/BlazorExtensions/SignalR
            services.AddTransient<HubConnectionBuilder>();
            services.AddTransient<IPhaserInterop, PhaserClient>();
            services.AddTransient<ApiClient>(); // TODO Refit?
            services
                .AddTransient<RanchScene>()
                .AddTransient<AnimalManagementScene>()
                .AddTransient<AnimalCareScene>()
                .AddTransient<SpinnerScene>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
            //app.UseStaticFiles();
        }
    }
}
