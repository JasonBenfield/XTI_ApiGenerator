using MainDB.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using XTI_App;
using XTI_WebApp.CodeGeneration;

namespace XTI_ApiGeneratorApp.Extensions
{
    public static class ApiGeneratorExtensions
    {
        public static void AddApiGenerator(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMainDbContextForSqlServer(configuration);
            services.AddScoped<AppFactory>();
            services.AddScoped(sp =>
            {
                var appFactory = sp.GetService<AppFactory>();
                var options = sp.GetService<IOptions<OutputOptions>>().Value;
                return new DefaultVersion(appFactory, options.DefaultVersion);
            });
            services.Configure<OutputOptions>(configuration.GetSection(OutputOptions.Output));
            services.AddScoped<ApiGenerator>();
        }
    }
}
