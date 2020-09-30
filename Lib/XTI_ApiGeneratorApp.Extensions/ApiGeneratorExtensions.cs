using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace XTI_ApiGeneratorApp.Extensions
{
    public static class ApiGeneratorExtensions
    {
        public static void ConfigureForApiGenerator(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OutputOptions>(configuration.GetSection(OutputOptions.Output));
            services.AddSingleton<ApiGenerator>();
        }
    }
}
