using Updater;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Updater;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.AddHostedService<Worker>();
    })
    .ConfigureLogging(builder =>
        builder.ClearProviders()
            .AddProvider(new UpdaterLoggerProvider(null)))
    .Build();

await host.RunAsync();
