using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace EmailVerificationService.Api.OpenApi;
// Provides an overall description of the API.
public sealed class OpenApiDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();

        document.Info.Title = "Email Verification Service API";
        document.Info.Version = "v1";
        document.Info.Description = """
        ## Introduction

        The Email Verification Service API provides endpoints for requesting and verifying email verification codes.
        
        The service is responsible for:
        - generating a temporary 6-digit verification code
        - storing the code until it expires
        - validating verification codes
        - publishing verification email messages to Azure Service Bus

        When an email verification is requested, the API creates a temporary verification code, stores it, and publishes a message to Azure Service Bus. The Email Sender Function then processes the queued message and sends the email through Azure Communication Services.
        
        """;

        return Task.CompletedTask;
    }
}
