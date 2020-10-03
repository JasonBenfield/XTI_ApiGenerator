using FakeWebApp.Api;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using XTI_ApiGeneratorApp.Extensions;

namespace FakeApiGeneratorApp
{
    public sealed class FakeApiGenerator : IHostedService
    {
        public FakeApiGenerator(IHostApplicationLifetime lifetime, ApiGenerator apiGenerator)
        {
            this.lifetime = lifetime;
            this.apiGenerator = apiGenerator;
        }

        private readonly IHostApplicationLifetime lifetime;
        private readonly ApiGenerator apiGenerator;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var apiTemplateFactory = new FakeAppApiTemplateFactory();
                var apiTemplate = apiTemplateFactory.Create();
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
