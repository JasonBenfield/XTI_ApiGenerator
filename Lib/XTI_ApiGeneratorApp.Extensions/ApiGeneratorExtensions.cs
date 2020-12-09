using MainDB.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XTI_App;

namespace XTI_ApiGeneratorApp.Extensions
{
    public static class ApiGeneratorExtensions
    {
        public static void AddApiGenerator(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAppDbContextForSqlServer(configuration);
            services.AddScoped<AppFactory>();
            services.Configure<OutputOptions>(configuration.GetSection(OutputOptions.Output));
            services.AddScoped<ApiGenerator>();
        }
    }
}
