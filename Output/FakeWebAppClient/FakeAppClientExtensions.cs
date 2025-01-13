// Generated Code
using Microsoft.Extensions.DependencyInjection;

namespace FakeWebAppClient;
public static class FakeAppClientExtensions
{
    public static void AddFakeAppClient(this IServiceCollection services)
    {
        services.TryAddScoped<IAppClientSessionKey, EmptyAppClientSessionKey>();
        services.TryAddScoped<IAppClientRequestKey, EmptyAppClientRequestKey>();
        services.AddScoped<FakeAppClientFactory>();
        services.AddScoped(sp => sp.GetRequiredService<FakeAppClientFactory>().Create());
        services.AddScoped<FakeAppClientVersion>();
    }
}