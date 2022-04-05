using Microsoft.Extensions.DependencyInjection;
using XTI_Core.Extensions;

namespace XTI_ApiGeneratorApp.Extensions;

public static class ApiGeneratorExtensions
{
    public static void AddApiGenerator(this IServiceCollection services)
    {
        services.AddConfigurationOptions<OutputOptions>(OutputOptions.Output);
        services.AddScoped<ApiGenerator>();
    }
}