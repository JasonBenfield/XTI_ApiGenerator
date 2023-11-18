// Generated Code
using Microsoft.Extensions.DependencyInjection;

namespace FakeWebAppClient;
public static class FakeAppClientExtensions
{
    public static void AddFakeAppClient(this IServiceCollection services)
    {
        services.AddScoped<FakeAppClient>();
        services.AddScoped<FakeAppClientVersion>();
    }
}