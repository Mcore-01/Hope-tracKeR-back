using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Hope_tracKeR_back.Config;

public static class CorsConfig
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(ConfigureCorsOptions);
    }

    private static void ConfigureCorsOptions(CorsOptions options)
    {
        options.AddPolicy("Development", builder => {
            builder.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    }
}