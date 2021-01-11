using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using XTI_App.Api;

namespace XTI_ApiGeneratorApp.Extensions
{
    public sealed class ApiGeneratorHostedService : IHostedService
    {
        private readonly IServiceScope scope;
        private readonly IHostApplicationLifetime lifetime;
        private readonly ApiGenerator apiGenerator;
        private readonly AppApiFactory appApiFactory;

        public ApiGeneratorHostedService(IServiceProvider sp)
        {
            scope = sp.CreateScope();
            lifetime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
            apiGenerator = scope.ServiceProvider.GetService<ApiGenerator>();
            appApiFactory = scope.ServiceProvider.GetService<AppApiFactory>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var apiTemplate = appApiFactory.CreateTemplate();
                await apiGenerator.Execute(apiTemplate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Environment.ExitCode = 999;
                Console.ReadLine();
            }
            Console.Out.Flush();
            Console.WriteLine("Done");
            lifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
