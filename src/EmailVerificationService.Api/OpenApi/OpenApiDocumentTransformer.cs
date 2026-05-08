using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace EmailVerificationService.Api.OpenApi;

public sealed class OpenApiDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();

        document.Info.Title = "Email Verification Service API";
        document.Info.Version = "v1";
        document.Info.Description = """
        ## Introduction

        The Email Verification Service API provides endpoints for email verification.
        
        """;

        return Task.CompletedTask;
    }
}
