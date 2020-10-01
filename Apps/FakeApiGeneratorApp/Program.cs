﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using XTI_Configuration.Extensions;
using XTI_ApiGeneratorApp.Extensions;
using XTI_ConsoleApp.Extensions;

namespace FakeApiGeneratorApp
{
    class Program
    {
        static Task Main(string[] args)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.UseXtiConfiguration(hostingContext.HostingEnvironment.EnvironmentName, args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddConsoleAppServices(hostContext.Configuration);
                    services.ConfigureForApiGenerator(hostContext.Configuration);
                    services.AddHostedService(sp =>
                    {
                        var scope = sp.CreateScope();
                        var lifetime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
                        var apiGenerator = scope.ServiceProvider.GetService<ApiGenerator>();
                        return new FakeApiGenerator(lifetime, apiGenerator);
                    });
                })
                .RunConsoleAsync();
        }
    }
}
