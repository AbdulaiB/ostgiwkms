using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ostgiwkms;
using ostgiwkms.Services;

namespace ostgiwkms
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<ApiService>();
            builder.Services.AddScoped<TextGenerationRequest>();
            builder.Services.AddScoped<SettingsService>();


            await builder.Build().RunAsync();
        }
    }
}