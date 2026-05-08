namespace EmailVerificationService.Api.Security;

public static class CorsConfiguration
{
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            // Allow requests from the frontend application Next.js running
            options.AddPolicy("Frontend", policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            });

        });

        return services;
    }
}

