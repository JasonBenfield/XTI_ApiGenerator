using FakeWebApp.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XTI_ApiGeneratorApp.Extensions;
using XTI_App.Api;
using XTI_Configuration.Extensions;

await Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.UseXtiConfiguration(hostingContext.HostingEnvironment, args);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddApiGenerator(hostContext.Configuration);
        services.AddScoped<AppApiFactory, FakeAppApiFactory>();
        services.AddHostedService<ApiGeneratorHostedService>();
    })
    .RunConsoleAsync();