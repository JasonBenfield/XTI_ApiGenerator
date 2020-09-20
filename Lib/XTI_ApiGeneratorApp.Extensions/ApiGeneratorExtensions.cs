using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace XTI_ApiGeneratorApp.Extensions
{
    public static class ApiGeneratorExtensions
    {
        public static void ConfigureForApiGenerator(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OutputOptions>(configuration.GetSection(OutputOptions.Output));
            services.AddSingleton(sp =>
            {
                var outputOptions = sp.GetService<IOptions<OutputOptions>>();
                return new ApiGenerator(outputOptions);
            });
        }
    }
}
