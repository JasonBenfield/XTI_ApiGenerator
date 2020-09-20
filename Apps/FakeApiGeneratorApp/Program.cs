using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using XTI_Configuration.Extensions;
using XTI_ApiGeneratorApp.Extensions;

namespace FakeApiGeneratorApp
{
    class Program
    {
        static Task Main(string[] args)
        {
            return new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.UseXtiConfiguration(hostingContext.HostingEnvironment.EnvironmentName, args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.ConfigureForApiGenerator(hostContext.Configuration);
                    services.AddHostedService(sp =>
                    {
                        var lifetime = sp.GetService<IHostApplicationLifetime>();
                        var apiGenerator = sp.GetService<ApiGenerator>();
                        return new FakeApiGenerator(lifetime, apiGenerator);
                    });
                })
                .RunConsoleAsync();
        }
    }
}
