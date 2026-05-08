
using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;

namespace EmailVerificationService.Api.OpenApi;
// OpenApi + Scalar setup for the API. This is separated from Program.cs to keep it clean.
public static class OpenApiConfiguration
{
    public static IServiceCollection AddOpenApiConfiguration(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<OpenApiDocumentTransformer>();
        });

        return services;
    }
}
