using Blazor.Extensions;
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
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
            //app.UseStaticFiles();
        }
    }
}
