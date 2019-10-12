using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace QuotationDance.Collector
{
    class Program
    {
        static void Main()
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    //configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddEnvironmentVariables("ASPNETCORE_");
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                    //services.AddSingleton<IHostedService>(n => new WebSocketCollectService(n.GetRequiredService<ILogger<WebSocketCollectService>>(), "huobipro"));
                    //services.AddSingleton<IHostedService>(n => new WebSocketCollectService(n.GetRequiredService<ILogger<WebSocketCollectService>>(), "okex"));
                    services.AddSingleton<IHostedService>(n => new WebSocketCollectService(n.GetRequiredService<ILogger<WebSocketCollectService>>(), "coinbasepro"));
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                    if (hostContext.HostingEnvironment.EnvironmentName == EnvironmentName.Development)
                    {
                        configLogging.AddDebug();
                    }
                })
                .UseConsoleLifetime()
                .Build();

            host.Run();
        }
    }
}
