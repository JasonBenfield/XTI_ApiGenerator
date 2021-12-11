using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XTI_App.Api;

namespace XTI_ApiGeneratorApp.Extensions;

public sealed class ApiGeneratorHostedService : IHostedService
{
    private readonly IServiceProvider sp;

    public ApiGeneratorHostedService(IServiceProvider sp)
    {
        this.sp = sp;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = sp.CreateScope();
        try
        {
            var appApiFactory = scope.ServiceProvider.GetRequiredService<AppApiFactory>();
            var apiTemplate = appApiFactory.CreateTemplate();
            var apiGenerator = scope.ServiceProvider.GetRequiredService<ApiGenerator>();
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
        var lifetime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
        lifetime.StopApplication();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}