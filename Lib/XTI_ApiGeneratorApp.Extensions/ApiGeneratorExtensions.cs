using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using XTI_WebApp.CodeGeneration;

namespace XTI_ApiGeneratorApp.Extensions
{
    public static class ApiGeneratorExtensions
    {
        public static void AddApiGenerator(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(sp =>
            {
                var options = sp.GetService<IOptions<OutputOptions>>().Value;
                return new DefaultVersion(options.DefaultVersion);
            });
            services.Configure<OutputOptions>(configuration.GetSection(OutputOptions.Output));
            services.AddScoped<ApiGenerator>();
        }
    }
}
